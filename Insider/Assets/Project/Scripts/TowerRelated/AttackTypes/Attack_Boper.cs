using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Boper : MonoBehaviour, IAttackType
{
    private bool isAttacking = false;
    private bool hasFiredThisCycle = false;
    private Dictionary<Enemy, Coroutine> bleedCoroutines = new Dictionary<Enemy, Coroutine>();

    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        if (isAttacking || anim == null) return;

        Tower tower = GetComponent<Tower>();
        if (e == null || e.Count == 0) return;

        anim.speed = tower.fireRate;
        anim.SetBool("IsAttacking", true);

        isAttacking = true;
        hasFiredThisCycle = false;

        StartCoroutine(HandleBopAttack(e, tower, anim));
    }

    private IEnumerator HandleBopAttack(List<Enemy> enemies, Tower tower, Animator anim)
    {
        float fireMoment = 0.666f;
        hasFiredThisCycle = false;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Attack"))
        {
            state = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        while (true)
        {
            if (tower.enemiesInRange == null || tower.enemiesInRange.Count == 0)
                break;

            state = anim.GetCurrentAnimatorStateInfo(0);
            float time = state.normalizedTime % 1f;

            if (time >= fireMoment && !hasFiredThisCycle)
            {
                ApplyEffect(enemies, tower);
                hasFiredThisCycle = true;
            }

            if (time < 0.1f)
                hasFiredThisCycle = false;

            if (state.IsName("Idle") || state.normalizedTime >= 1f)
                break;

            yield return null;
        }

        anim.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    private void ApplyEffect(List<Enemy> e, Tower tower)
    {
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
                    StartCoroutine(ApplySlow(enemy, 0.5f, 0.5f / tower.fireRate));
                if (tower.currentLevel == 3)
                    StartCoroutine(ApplySlow(enemy, 0, 0.5f / tower.fireRate));
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
    }

    private IEnumerator ApplySlow(Enemy enemy, float slowFactor, float duration)
    {
        if (enemy == null) yield break;

        float originalSpeed = enemy.movSpeed;
        enemy.movSpeed *= slowFactor;

        yield return new WaitForSeconds(duration);

        if (enemy != null)
            enemy.movSpeed = originalSpeed;
    }

    private IEnumerator ApplyBleed(Tower t, Enemy enemy, float damagePerTick, int ticks)
    {
        if (enemy == null) yield break;

        if (t.currentLevel == 3)
            enemy.isBleeding = true;

        for (int i = 0; i < ticks; i++)
        {
            if (enemy == null) break;
            enemy.GetComponent<IDamage>().Damage(damagePerTick);
            yield return new WaitForSeconds(2f);
        }

        if (enemy != null)
        {
            if (t.currentLevel == 3)
                enemy.isBleeding = false;

            if (bleedCoroutines.ContainsKey(enemy))
                bleedCoroutines.Remove(enemy);
        }
    }
}