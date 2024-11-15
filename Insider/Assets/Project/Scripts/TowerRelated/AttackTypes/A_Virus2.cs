using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class A_Virus2 : MonoBehaviour, IAttackType
{
    private Enemy target;

    private string assetAddress = "Prefabs/BulletChomp";
    private GameObject chompPrefab;
    private GameObject chomp;
    private Vector2 direction;
    private float angle;
    private float distance;
    private float extractSpeed;
    private float retractSpeed;

    float totalDistance;

    IDamage enemyDmg;

    void Start()
    {
        extractSpeed = 0.5f;
        retractSpeed = extractSpeed / 2;
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            chompPrefab = handle.Result;
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
        enemyDmg = fire.transform.GetComponent<IDamage>();
        DrawTongue();
        
    }

    void DrawTongue()
    {
        chomp = Instantiate(chompPrefab, transform.position, Quaternion.identity);
        chomp.GetComponent<SpriteRenderer>().size = new Vector2(0, chomp.GetComponent<SpriteRenderer>().size.y);
        StartCoroutine(ExtractTongue());
    }

    IEnumerator ExtractTongue()
    {
        totalDistance = Vector2.Distance(transform.position, target.transform.position);
        chomp.GetComponent<SpriteRenderer>().size = new Vector2(0, chomp.GetComponent<SpriteRenderer>().size.y);

        while (chomp.GetComponent<SpriteRenderer>().size.x < totalDistance && target != null)
        {
            totalDistance = Vector2.Distance(transform.position, target.transform.position);

            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            Vector2 newSize = new Vector2(chomp.GetComponent<SpriteRenderer>().size.x + extractSpeed / GetComponent<Tower>().fireRate, chomp.GetComponent<SpriteRenderer>().size.y);

            if (newSize.x > totalDistance)
            {
                newSize.x = totalDistance;
            }

            chomp.GetComponent<SpriteRenderer>().size = newSize;
            chomp.transform.position = (Vector2)transform.position + directionToTarget * (chomp.GetComponent<SpriteRenderer>().size.x / 2);
            chomp.transform.rotation = Quaternion.Euler(0, 0, targetAngle);

            yield return null;
        }

        yield return StartCoroutine(RetractTongue());
    }

    IEnumerator RetractTongue()
    {
        Vector3 targetPos = target.transform.position;
        if (enemyDmg != null)
        {
            enemyDmg.Damage(target.health);
        }
        while (chomp.GetComponent<SpriteRenderer>().size.x > 0)
        {
            Vector2 newSize = new Vector2(chomp.GetComponent<SpriteRenderer>().size.x - retractSpeed / GetComponent<Tower>().fireRate, chomp.GetComponent<SpriteRenderer>().size.y);

            if (newSize.x < 0)
            {
                newSize.x = 0;
            }

            chomp.GetComponent<SpriteRenderer>().size = newSize;

            Vector2 directionToTarget = (targetPos - transform.position).normalized;
            chomp.transform.position = (Vector2)transform.position + directionToTarget * (chomp.GetComponent<SpriteRenderer>().size.x / 2);

            yield return null;
        }

        Destroy(chomp);
    }
}
