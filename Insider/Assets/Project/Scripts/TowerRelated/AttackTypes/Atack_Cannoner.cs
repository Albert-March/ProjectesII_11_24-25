using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Atack_Cannoner : MonoBehaviour, IAttackType
{
	public GameObject normalBulletPrefab;
	public GameObject chainBulletPrefab;

	private string assetAddress = "Prefabs/Bullet2";
	private string assetAddress2 = "Prefabs/ChainBullet";
	private float lastShotTime = 0f;

	void Awake()
	{
		// NormalBullt
		Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += handle =>
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
				normalBulletPrefab = handle.Result;
		};

		// ChainBullet
		Addressables.LoadAssetAsync<GameObject>(assetAddress2).Completed += handle =>
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
				chainBulletPrefab = handle.Result;
		};
	}

	public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
	{
		Tower tower = GetComponent<Tower>();

		if ((normalBulletPrefab == null && tower.type == 0) || (chainBulletPrefab == null && tower.type == 1)) return;
		if (e == null || e.Count == 0) return;
		if (Time.time < lastShotTime + 1f / tower.fireRate) return;

		List<Enemy> enemyHolder = targetManager.GetEnemyTargetFromList(e, TargetAmount, targetType);
		if (enemyHolder.Count == 0 || enemyHolder[0] == null) return;

		if (tower.type == 0)
		{
			// Dispar NormalBullt
			GameObject bullet = Instantiate(normalBulletPrefab, transform.position, Quaternion.identity);
			NormalBullet bulletScript = bullet.GetComponent<NormalBullet>();
			bulletScript.towerScript = tower;
			bulletScript.SetTarget(enemyHolder[0]);
		}
		else if (tower.type == 1)
		{
			// Dispar ChainBullet
			List<Enemy> chainTargets = new List<Enemy>();
			Enemy initialTarget = enemyHolder[0];
			chainTargets.Add(initialTarget);

			int bounces = 2;
			Enemy current = initialTarget;
			HashSet<Enemy> alreadyHit = new HashSet<Enemy> { current };

			for (int i = 0; i < bounces; i++)
			{
				Enemy nextTarget = null;
				float minDist = float.MaxValue;

				foreach (Enemy enemy in e)
				{
					if (enemy == null || enemy.gameObject == null || alreadyHit.Contains(enemy)) continue;

					float dist = Vector3.Distance(current.transform.position, enemy.transform.position);
					if (dist < minDist)
					{
						minDist = dist;
						nextTarget = enemy;
					}
				}

				if (nextTarget != null)
				{
					chainTargets.Add(nextTarget);
					alreadyHit.Add(nextTarget);
					current = nextTarget;
				}
				else
				{
					break;
				}
			}

			GameObject bullet = Instantiate(chainBulletPrefab, transform.position, Quaternion.identity);
			ChainBullet bulletScript = bullet.GetComponent<ChainBullet>();
			bulletScript.towerScript = tower;
			bulletScript.SetTargets(chainTargets);
		}

		lastShotTime = Time.time;
	}
}
