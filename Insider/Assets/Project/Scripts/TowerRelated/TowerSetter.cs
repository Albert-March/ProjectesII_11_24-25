using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSetter : MonoBehaviour
{
	public List<TowerStats> towerStats;
	public List<TowerStats> towerUpgrades1;
	public List<TowerStats> towerUpgrades2;

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
        tower.enabled = true;

    }

	public void LevelUp2(Tower tower)
	{
		TowerStats stats = towerUpgrades1[tower.id];
		tower.LevelUp(stats);
		
	}

	public void LevelUp3(Tower tower)
	{
		TowerStats stats = towerUpgrades2[tower.id];
		tower.LevelUp(stats);
	}
}
