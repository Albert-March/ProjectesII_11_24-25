using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class A_Virus2 : MonoBehaviour, IAttackType
{
    private Enemy target;

    private string assetAddress = "Prefabs/BulletLaser";
    private GameObject laserPrefab;
    private GameObject laser;
    private Vector2 direction;
    private float angle;
    private float distance;
    private float extractSpeed = 10;
    private float retractSpeed = 2;

    private bool canDelete = false;

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
        RaycastHit2D fire = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.blue);
        Debug.Log(fire.transform.name);
        IDamage enemyDmg = fire.transform.GetComponent<IDamage>();
        DrawTongue();
        if (enemyDmg != null && canDelete)
        {
            enemyDmg.Damage(e.health);
        }
        
    }

    void DrawTongue()
    {
        laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        laser.GetComponent<SpriteRenderer>().size = new Vector2(distance, laser.GetComponent<SpriteRenderer>().size.y);
        StartCoroutine(ExtractTongue());
    }
    IEnumerator ExtractTongue() 
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);

        while (laser.transform.position != (target.transform.position - transform.position) / 2 && laser.GetComponent<SpriteRenderer>().size.x < Mathf.Abs(distance))
        {
            Vector2 updateDirection = (target.transform.position - transform.position).normalized;
            float updateAngle = Mathf.Atan2(updateDirection.y, updateDirection.x) * Mathf.Rad2Deg;

            Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.blue);
            laser.GetComponent<SpriteRenderer>().size = new Vector2(laser.GetComponent<SpriteRenderer>().size.x + (GetComponent<Tower>().fireRate / extractSpeed) * Time.deltaTime, laser.GetComponent<SpriteRenderer>().size.y);
            laser.transform.position = Vector2.MoveTowards(laser.transform.position, Vector3.Lerp(transform.position, target.transform.position, 0.5f), 0.1f);
            laser.transform.rotation = Quaternion.Euler(0, 0, updateAngle);
            yield return null;
        }
        canDelete = true;
    }
    void RetractTongue() 
    {
    
    }

    IEnumerator DeleatLaser()
    {
        while (laser.transform.localScale.y > 0)
        {
            laser.transform.localScale = new Vector2(1, laser.transform.localScale.y - ((GetComponent<Tower>().fireRate / 2) * Time.deltaTime));
            yield return null;
        }
        Destroy(laser);

    }
}
