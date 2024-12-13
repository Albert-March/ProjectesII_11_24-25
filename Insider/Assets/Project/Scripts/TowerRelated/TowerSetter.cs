using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSetter : MonoBehaviour
{
	public List<TowerStats> towerStats;

	public void SpawnTower(int option, Transform towerPos)
	{
		if (option < 0 || option > towerStats.Count)
		{
			Debug.LogWarning("Opción de torre inválida.");
			return;
		}

		TowerStats stats = towerStats[option];
        GameObject towerObject = Instantiate(stats.prefab, towerPos.position, Quaternion.identity);
        towerObject.transform.SetParent(towerPos, true);
        towerObject.transform.rotation = towerPos.rotation;
        Tower tower = towerObject.GetComponent<Tower>();
        tower.SetTowerData(stats);

	}
}
