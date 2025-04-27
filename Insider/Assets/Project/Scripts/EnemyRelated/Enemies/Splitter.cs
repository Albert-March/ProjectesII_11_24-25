using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Splitter : MonoBehaviour, IRewardDropper
{
	private string assetAddress = "Prefabs/EnemyPrefab";
	public GameObject enemyPrefab;

	private EnemyManager enemyManager;
	private TargetManager targetManager;
	private EnemyStats normalEnemyStats;

	private void Start()
	{
		enemyManager = FindObjectOfType<EnemyManager>();
		targetManager = FindObjectOfType<TargetManager>();

		// Obtener las estadísticas del enemigo "Normal" desde el EnemyTypeManager o directamente
		normalEnemyStats = FindNormalEnemyStats();

		// Cargar el prefab mediante Addressables
		Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
	}

	private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			enemyPrefab = handle.Result;
		}
	}

	private EnemyStats FindNormalEnemyStats()
	{
		EnemyStats[] allStats = Resources.FindObjectsOfTypeAll<EnemyStats>();
		for (int i = 0; i < allStats.Length; i++)
		{
			if (allStats[i].id == 0)
			{
				return allStats[i];
			}
		}
		return null;
	}

	public void SpawnReward(List<Target> path)
	{
		// Obtener la referencia al componente Enemy del Splitter
		Enemy splitterEnemy = GetComponent<Enemy>();
		List<Vector3> spawnPositions = new List<Vector3>();
		Vector3 pos1 = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
		Vector3 pos2 = new Vector3(transform.position.x - 0.05f, transform.position.y + 0.05f, transform.position.z);
		Vector3 pos3 = new Vector3(transform.position.x + 0.05f, transform.position.y + 0.05f, transform.position.z);
		spawnPositions.Add(pos1);
		spawnPositions.Add(pos2);
		spawnPositions.Add(pos3);

		for (int i = 0; i < 3; i++)
		{
			GameObject enemyInstance = Instantiate(enemyPrefab, spawnPositions[i], Quaternion.identity);

			Enemy enemyScript = enemyInstance.GetComponent<Enemy>();

			enemyScript.SetEnemyData(normalEnemyStats);
			enemyScript.enemyManager = enemyManager;

			enemyScript.path = new List<Target>(splitterEnemy.path);
			enemyScript.currentTarget = splitterEnemy.currentTarget;

			enemyScript.enabled = true;

			enemyManager.AddSpawnedEnemy(enemyScript);
			enemyManager.wavesInfo.simulatedEnemies.Add('1');

        }
	}
}
