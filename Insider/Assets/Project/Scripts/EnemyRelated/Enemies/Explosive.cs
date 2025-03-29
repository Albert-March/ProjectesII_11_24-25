using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Explosive : MonoBehaviour, IRewardDropper
{
	private string assetAddress = "Prefabs/HealingEnemy";
	public GameObject reward;

	private void Start()
	{
		Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
		
	}

	private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			reward = handle.Result;
		}
	}
	public void SpawnReward(List<Target> path)
	{
		GameObject rewardInstance = Instantiate(reward, transform.position, Quaternion.identity);
	}
}
