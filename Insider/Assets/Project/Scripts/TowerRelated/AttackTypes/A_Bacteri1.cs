using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class A_Bacteri1 : MonoBehaviour, IAttackType
{
    private GameObject bulletPrefab;
    private string assetAddress = "Prefabs/Bullet1";

    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            bulletPrefab = handle.Result;
        }
    }
    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // Asignar objetiu
        ExplosiveBullet bulletScript = bullet.GetComponent<ExplosiveBullet>();
        bulletScript.towerScript = GetComponent<Tower>();
        List<Enemy> enemyHolder = targetManager.GetEnemyTargetFromList(e, TargetAmount, targetType);
        bulletScript.SetTarget(enemyHolder[0]);
    }
}
