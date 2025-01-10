using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class A_Virus1 : MonoBehaviour, IAttackType
{
    private Enemy target;
    
    private string assetAddress = "Prefabs/BulletLaser";
    private GameObject laserPrefab;
    private GameObject laser;
    private Vector2 direction;
    private float angle;
    private float distance;

    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            laserPrefab = handle.Result;
        }
    }
    public void Attack(Enemy e)
    {
        if (e != null) { target = e; }
        direction = (target.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        distance = Vector2.Distance(transform.position, target.transform.position);

        DrawLaser();

        IDamage enemyDmg = e.transform.GetComponent<IDamage>();
        if (enemyDmg != null)
        {
            enemyDmg.Damage(GetComponent<Tower>().damage);
        }
    }

    void DrawLaser()
    {
        if (target == null) { return; }
        laser = Instantiate(laserPrefab, Vector3.Lerp(transform.position, target.transform.position, 0.5f), Quaternion.Euler(0, 0, angle));
        laser.transform.SetParent(this.transform);
        laser.GetComponent<SpriteRenderer>().size = new Vector2(distance, laser.GetComponent<SpriteRenderer>().size.y);
        StartCoroutine(DeleatLaser());
    }

    IEnumerator DeleatLaser()
    {
        while (laser.transform.localScale.y > 0) 
        {
            laser.transform.localScale = new Vector2(1, laser.transform.localScale.y - ((GetComponent<Tower>().fireRate/2) * Time.deltaTime));
            yield return null;
        }
        Destroy(laser);

    }
}

