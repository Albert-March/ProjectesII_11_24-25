using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Atack_Cannoner : MonoBehaviour, IAttackType
{
    public GameObject normalBulletPrefab;
    public GameObject exploBulletPrefab;
    public GameObject chainBulletPrefab;

    private string assetAddress0 = "Prefabs/Bullet1";
    private string assetAddress = "Prefabs/Bullet2";
    private string assetAddress2 = "Prefabs/ChainBullet";

    private bool isAttacking = false;
    private bool hasFiredThisCycle = false;
    private bool movingArm;

    public Transform Arm1Point;
    public Transform Arm2Point;
    public GameObject BaseCannoner;

    void Awake()
    {
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                normalBulletPrefab = handle.Result;
        };

        Addressables.LoadAssetAsync<GameObject>(assetAddress2).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                chainBulletPrefab = handle.Result;
        };

        Addressables.LoadAssetAsync<GameObject>(assetAddress0).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                exploBulletPrefab = handle.Result;
        };
    }

    public static GameObject FindDeepChildByName(GameObject parent, string targetName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == targetName)
                return child.gameObject;

            GameObject result = FindDeepChildByName(child.gameObject, targetName);
            if (result != null)
                return result;
        }
        return null;
    }

    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        Arm1Point = FindDeepChildByName(gameObject, "AP1").transform;
        Arm2Point = FindDeepChildByName(gameObject, "AP2").transform;
        BaseCannoner = FindDeepChildByName(gameObject, "Cannoner_1");
        if (isAttacking || !anim) return;

        Tower tower = GetComponent<Tower>();

        if ((normalBulletPrefab == null && tower.type == 0) ||
            (chainBulletPrefab == null && tower.type == 1) ||
            (exploBulletPrefab == null && tower.type == 2)) return;

        if (e == null || e.Count == 0) return;

        anim.speed = tower.fireRate;
        anim.SetBool("IsAttacking", true);

        isAttacking = true;
        hasFiredThisCycle = false;

        StartCoroutine(HandleFire(tower, tower.type, tower.targetManager, anim));
    }

    IEnumerator HandleFire(Tower tower, int type, TargetingManager targetManager, Animator anim)
    {
        float fireMoment = 0.666f;
        hasFiredThisCycle = false;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!(state.IsName("Attack") || state.IsName("Attack2")))
        {
            state = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        while (true)
        {
            if (tower.enemiesInRange == null || tower.enemiesInRange.Count == 0)
                break;

            Enemy mainTarget = tower.enemiesInRange[0];
            if (mainTarget != null)
            {
                Vector3 dir = (mainTarget.transform.position - BaseCannoner.transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                BaseCannoner.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            }

            state = anim.GetCurrentAnimatorStateInfo(0);
            float time = state.normalizedTime % 1f;

            if (time >= fireMoment && !hasFiredThisCycle)
            {
                List<Enemy> updatedTargets = targetManager.GetEnemyTargetFromList(tower.enemiesInRange, tower.targetAmount, tower.targetType);
                if (updatedTargets.Count > 0 && updatedTargets[0] != null)
                {
                    FireBullet(type, updatedTargets, tower);
                    hasFiredThisCycle = true;
                }
                else
                {
                    break;
                }
            }

            if (time < 0.1f)
                hasFiredThisCycle = false;

            if (state.IsName("Attack")) movingArm = true;
            else if (state.IsName("Attack2")) movingArm = false;

            yield return null;
        }

        anim.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    void FireBullet(int type, List<Enemy> targets, Tower tower)
    {
        Vector3 spawnPos = movingArm ? Arm1Point.position : Arm2Point.position;

        if (type == 0)
        {
            var bullet = Instantiate(normalBulletPrefab, spawnPos, Quaternion.identity).GetComponent<NormalBullet>();
            bullet.towerScript = tower;
            bullet.SetTarget(targets[0]);
        }
        else if (type == 1)
        {
            List<Enemy> chainTargets = new() { targets[0] };
            Enemy current = targets[0];
            HashSet<Enemy> hit = new() { current };

            for (int i = 1; i < Mathf.Min(tower.targetAmount, targets.Count); i++)
            {
                Enemy next = null;
                float minDist = float.MaxValue;
                foreach (Enemy candidate in targets)
                {
                    if (hit.Contains(candidate)) continue;
                    float dist = Vector3.Distance(current.transform.position, candidate.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        next = candidate;
                    }
                }
                if (next == null) break;
                chainTargets.Add(next);
                hit.Add(next);
                current = next;
            }

            var bullet = Instantiate(chainBulletPrefab, spawnPos, Quaternion.identity).GetComponent<ChainBullet>();
            bullet.towerScript = tower;
            bullet.SetTargets(chainTargets);
        }
        else if (type == 2)
        {
            var bullet = Instantiate(exploBulletPrefab, spawnPos, Quaternion.identity).GetComponent<ExplosiveBullet>();
            bullet.towerScript = tower;
            bullet.SetTarget(targets[0]);
        }
    }
}