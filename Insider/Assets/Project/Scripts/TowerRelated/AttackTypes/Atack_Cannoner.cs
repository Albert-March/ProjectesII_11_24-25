using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Atack_Cannoner : MonoBehaviour, IAttackType
{
    public GameObject bulletPrefab;
    private string assetAddress = "Prefabs/Bullet2";

    void Awake()
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
        NormalBullet bulletScript = bullet.GetComponent<NormalBullet>();
        bulletScript.towerScript = GetComponent<Tower>();
        List<Enemy> enemyHolder = targetManager.GetEnemyTargetFromList(e, TargetAmount, targetType);
        bulletScript.SetTarget(enemyHolder[0]);
    }
}
