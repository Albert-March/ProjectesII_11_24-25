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

    private bool isPrefabLoaded = false;

    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            chompPrefab = handle.Result;
            isPrefabLoaded = true;
        }
        else
        {
            Debug.LogError("Failed to load Chomp prefab.");
        }
    }

    public void Attack(Enemy e)
    {
        if (!isPrefabLoaded)
        {
            Debug.LogWarning("Chomp prefab not loaded yet.");
            return;
        }

        if (e != null)
        {
            target = e;
        }


        if (target == null)
        {
            Debug.LogWarning("No target assigned.");
            return;
        }


        DrawTongue();
    }

    void DrawTongue()
    {
        if (chompPrefab == null)
        {
            Debug.LogError("Chomp Prefab not loaded.");
            return;
        }

        chomp = Instantiate(chompPrefab, transform.position, Quaternion.identity);
        chomp.GetComponent<TongeBullet>().target = target;
    }


}