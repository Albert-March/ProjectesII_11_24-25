using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSetter : MonoBehaviour
{
	public List<TowerStats> towerStats;
	public List<TowerStats> towerUpgrades1Type1;
	public List<TowerStats> towerUpgrades1Type2;

	public List<TowerStats> towerUpgrades2Type1;
	public List<TowerStats> towerUpgrades2Type2;

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
		if (option == 1) 
		{
			towerObject.transform.localPosition += new Vector3(0, 45, 0);
        }
        Tower tower = towerObject.GetComponent<Tower>();
        tower.SetTowerData(stats);
        tower.enabled = true;

    }

	public void LevelUp2(Tower tower)
	{
		TowerStats stats;
		if (tower.type == 1)
		{
			stats = towerUpgrades1Type1[tower.id];
		}
		else
		{
			stats = towerUpgrades1Type2[tower.id];
		}
		tower.LevelUp(stats);	
	}

	public void LevelUp3(Tower tower)
	{
		TowerStats stats;
		if (tower.type == 1)
		{
			stats = towerUpgrades2Type1[tower.id];
		}
		else
		{
			stats = towerUpgrades2Type2[tower.id];
		}
		tower.LevelUp(stats);
	}
}
