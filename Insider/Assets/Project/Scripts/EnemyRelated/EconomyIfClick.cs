using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EconomyIfClick : MonoBehaviour
{
	private string assetAddress = "Prefabs/RewardPrefab";
	public GameObject reward;
	private List<Target> path;

	private GameObject rewardInstance;
	EconomyManager economyScript;

	public SpriteRenderer spriteRenderer;

	private float timer = 0f;

	private void Start()
	{
		Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
	{
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			reward = handle.Result;
		}
	}
	public void SetPath(List<Target> receivedPath)
	{
		path = receivedPath;
	}
	public void Collect()
	{
		economyScript = FindObjectOfType<EconomyManager>();
		economyScript.economy += 50;
		Destroy(gameObject);
		rewardInstance = Instantiate(reward, transform.position, Quaternion.identity);
		RewardManager rewardManager = rewardInstance.GetComponent<RewardManager>();
		Vector3 vector3 = path[path.Count - 1].obj.transform.position;
		rewardManager.Initialize(vector3);
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer >= 5f)
		{
			Destroy(gameObject);
		}
	}
}
