using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackManager : MonoBehaviour
{
	public CircleCollider2D detectionCollider;
	public GameObject bulletPrefab;
	public Transform firePoint;

	private float lastShootTime;

	public List<GameObject> enemiesInRange = new List<GameObject>();

	GameObject targetEnemy;
	public Tower towerScript;
	private void Start()
	{
		towerScript = GetComponent<Tower>();
		lastShootTime = -towerScript.fireRate;
	}

	private void Update()
	{
		if (enemiesInRange.Count > 0)
		{
			targetEnemy = enemiesInRange[0];
			if (detectionCollider.IsTouching(targetEnemy.GetComponent<Collider2D>()) && Time.time >= lastShootTime + towerScript.fireRate)
			{
				ShootAtTarget(targetEnemy);
				lastShootTime = Time.time;
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Enemy")
		{
			if (detectionCollider.IsTouching(other))
			{
				if (!enemiesInRange.Contains(other.gameObject))
				{
					enemiesInRange.Add(other.gameObject);
				}
			}
		}
		
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			if (!detectionCollider.IsTouching(other))
			{
				if (enemiesInRange.Contains(other.gameObject))
				{
					enemiesInRange.Remove(other.gameObject);
				}
			}
		}
	}

	void ShootAtTarget(GameObject target)
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
		// Asignar objetiu
		Bullet bulletScript = bullet.GetComponent<Bullet>();
		bulletScript.towerScript = towerScript;
		if (bulletScript != null)
		{
			bulletScript.SetTarget(target);
		}
	}
}
