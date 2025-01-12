using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Normal : MonoBehaviour, IRewardDropper
{
	private string assetAddress = "Prefabs/RewardPrefab";
	public GameObject reward;
	private GameObject rewardInstance;

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
		rewardInstance = Instantiate(reward, transform.position, Quaternion.identity);
        RewardManager rewardManager = rewardInstance.GetComponent<RewardManager>();
        Vector3 vector3 = path[path.Count - 1].obj.transform.position;
        rewardManager.Initialize(vector3);
    }
}
