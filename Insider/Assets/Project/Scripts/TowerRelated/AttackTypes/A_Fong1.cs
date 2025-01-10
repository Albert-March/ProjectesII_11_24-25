using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class A_Fong1 : MonoBehaviour, IAttackType
{
    public CircleCollider2D circleCollider;

    public List<GameObject> amountOfTagsCreated = new List<GameObject>();

    public GameObject bulletPrefab;
    private string assetAddress = "Prefabs/Bullet3";

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
    public void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void Attack(Enemy e)
    {
        if (amountOfTagsCreated.Count < GetComponent<Tower>().projectileSpeed) 
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // Asignar objetiu
            bullet.transform.SetParent(this.transform);
            TagBullet bulletScript = bullet.GetComponent<TagBullet>();
            bulletScript.towerScript = GetComponent<Tower>();
            amountOfTagsCreated.Add(bullet);
            bulletScript.SetTarget(GetRandomPointInsideCircle(), this);
        }

    }

    Vector2 GetRandomPointInsideCircle()
    {
        Vector2 randomPoint = new Vector2(0,0);
        bool validPoint = false;

        while (!validPoint)
        {
            // Step 1: Generate a random point inside the circle collider
            randomPoint = RandomPointInCircle();

            // Step 2: Check if the point collides with any collider in the avoid layer mask
            Collider2D hitCollider = Physics2D.OverlapPoint(randomPoint, LayerMask.GetMask("Terrain"));

            // Step 3: If no collider is hit, the point is valid
            if (hitCollider == null)
            {
                validPoint = true;
            }
        }
        return randomPoint;
    }

    Vector2 RandomPointInCircle()
    {
        // Generate a random point within a circle's radius
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * circleCollider.radius*2;

        // Calculate the x and y position based on the angle and radius
        Vector2 point = new Vector2(
            Mathf.Cos(angle) * radius,
            Mathf.Sin(angle) * radius
        );

        // Offset by the position of the circle collider center
        return point + (Vector2)circleCollider.transform.position;
    }
}
