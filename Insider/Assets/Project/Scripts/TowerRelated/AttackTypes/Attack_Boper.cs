using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Boper : MonoBehaviour, IAttackType
{
    private float lastShotTime = 0f;
    private Dictionary<Enemy, Coroutine> bleedCoroutines = new Dictionary<Enemy, Coroutine>();


    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        Tower tower = GetComponent<Tower>();
        if (e == null || e.Count == 0) return;
        if (Time.time < lastShotTime + 1f / tower.fireRate) return;

        foreach (Enemy enemy in e)
        {
            if (enemy == null) continue;

            if (tower.type == 0)
            {
                enemy.GetComponent<IDamage>().Damage(tower.damage);
            }
            else if (tower.type == 1)
            {
                enemy.GetComponent<IDamage>().Damage(tower.damage);
                if (tower.currentLevel == 2) 
                {
                    StartCoroutine(ApplySlow(enemy, 0.5f, 0.5f / tower.fireRate));
                }
                if (tower.currentLevel == 3) 
                {
                    StartCoroutine(ApplySlow(enemy, 0, 0.5f / tower.fireRate));
                }

            }
            else if (tower.type == 2)
            {
                if (bleedCoroutines.ContainsKey(enemy))
                {
                    StopCoroutine(bleedCoroutines[enemy]);
                    bleedCoroutines.Remove(enemy);
                }

                Coroutine bleed = StartCoroutine(ApplyBleed(tower, enemy, 2f, 10));
                bleedCoroutines[enemy] = bleed;

                enemy.GetComponent<IDamage>().Damage(tower.damage);
            }
        }

        lastShotTime = Time.time;
    }

    private IEnumerator ApplySlow(Enemy enemy, float slowFactor, float duration)
    {
        if (enemy == null) yield break;

        float originalSpeed = enemy.movSpeed;
        enemy.movSpeed *= slowFactor;

        yield return new WaitForSeconds(duration);

        if (enemy != null)
        {
            enemy.movSpeed = originalSpeed;
        }
    }

    private IEnumerator ApplyBleed(Tower t,Enemy enemy, float damagePerTick, int ticks)
    {
        if (enemy == null) yield break;

        if (t.currentLevel == 3)
        {
            enemy.isBleeding = true;
        }

        for (int i = 0; i < ticks; i++)
        {
            if (enemy == null) break;
            enemy.GetComponent<IDamage>().Damage(damagePerTick);
            yield return new WaitForSeconds(2f);
        }

        if (enemy != null)
        {
            if (t.currentLevel == 3)
            {
                enemy.isBleeding = false;
            }
            if (bleedCoroutines.ContainsKey(enemy))
                bleedCoroutines.Remove(enemy);
        }
    }
}
