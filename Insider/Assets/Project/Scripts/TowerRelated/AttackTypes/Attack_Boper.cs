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

        StartCoroutine(HandleBopAttack(e, tower, anim, audio));
    }

    private IEnumerator HandleBopAttack(List<Enemy> enemies, Tower tower, Animator anim, AudioManager audio)
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
            { 
                anim.SetBool("IsAttacking", false);
                isAttacking = false;
                break; 
            }

            state = anim.GetCurrentAnimatorStateInfo(0);
            float time = state.normalizedTime % 1f;

            if (time >= fireMoment && !hasFiredThisCycle && state.IsName("Attack"))
            {
                ApplyEffect(enemies, tower);
                audio.PlaySFX(8, 0.2f);
                hasFiredThisCycle = true;
            }

            if (state.IsName("Recoil"))
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
        for (int i = 0; i < e.Count; i++)
        {
            Enemy enemy = e[i];
            if (enemy == null) continue;

            if (tower.type == 0)
            {
                enemy.GetComponent<IDamage>().Damage(tower.damage);
            }
            else if (tower.type == 1)
            {
                enemy.GetComponent<IDamage>().Damage(tower.damage);
                if (tower.currentLevel == 2)
                    StartCoroutine(ApplySlow(enemy, 0.5f / tower.fireRate));
                if (tower.currentLevel == 3)
                    StartCoroutine(ApplyStun(enemy, 0.5f / tower.fireRate));
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

    private IEnumerator ApplySlow(Enemy enemy, float duration)
    {
        if (enemy == null) yield break;

        enemy.isSlowed = true;

        yield return new WaitForSeconds(duration);

        if (enemy != null)
            enemy.isSlowed = false;
    }

    private IEnumerator ApplyStun(Enemy enemy, float duration)
    {
        if (enemy == null) yield break;

        enemy.isStuned = true;

        yield return new WaitForSeconds(duration);

        if (enemy != null)
            enemy.isStuned = false;
    }

    private IEnumerator ApplyBleed(Tower t, Enemy enemy, float damagePerTick, int ticks)
    {
        if (enemy == null) yield break;

        if (t.currentLevel == 3)
            enemy.isWeek = true;

        for (int i = 0; i < ticks; i++)
        {
            if (enemy == null) break;
            enemy.GetComponent<IDamage>().Damage(damagePerTick);
            enemy.isBleeding = true;
            yield return new WaitForSeconds(2f);
        }

        if (enemy != null)
        {
            if (t.currentLevel == 3)
                enemy.isWeek = false;

            if (bleedCoroutines.ContainsKey(enemy))
                enemy.isBleeding = false;
                bleedCoroutines.Remove(enemy);
        }
    }
}