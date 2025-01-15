using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTowerBaseInput : MonoBehaviour
{
	[SerializeField] private ParticleSystem levelUpParticles;
	private ParticleSystem levelUpParticlesInstance;

	public TowerSetter towerSetter;
	public GameObject clickedButton;
	public GameObject cam;

    public int towerGrup;
    public bool spawnTower;
	bool levelUp2;
	bool levelUp3;


    EconomyManager economyScript;
	StatesManager states;
    private void Update()
    {
		if (clickedButton != null) 
		{
			cam.transform.position = new Vector3(clickedButton.transform.position.x, clickedButton.transform.position.y, -10);

            cam.transform.rotation = clickedButton.transform.rotation;

            towerGrup = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().towerId;
            spawnTower = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower;
            levelUp2 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2;
            levelUp3 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3;
        }
    }
    public void LlamarSpawnTowerOpcion1()
	{
        
        if (!spawnTower)
		{
			economyScript = FindObjectOfType<EconomyManager>();
			states = FindObjectOfType<StatesManager>();

			if (economyScript.economy >= 300)
			{

				switch (towerGrup)
				{
					case 0:
						towerSetter.SpawnTower(0, clickedButton.transform);
						break;
					case 1:
						towerSetter.SpawnTower(2, clickedButton.transform);
						break;
					case 2:
						towerSetter.SpawnTower(4, clickedButton.transform);
						break;
				}
				economyScript.economy -= 300;
                clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower = true;
            }
		}
	}

	public void LlamarSpawnTowerOpcion2()
	{
        if (!spawnTower)
		{
			economyScript = FindObjectOfType<EconomyManager>();
			states = FindObjectOfType<StatesManager>();
			if (economyScript.economy >= 300)
			{
				switch (towerGrup)
				{
					case 0:
						towerSetter.SpawnTower(1, clickedButton.transform);
						break;
					case 1:
						towerSetter.SpawnTower(3, clickedButton.transform);
						break;
					case 2:
						towerSetter.SpawnTower(5, clickedButton.transform);
						break;
				}
				economyScript.economy -= 300;
                clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower = true;
            }
		}
	}

	public void LevelUp2()
	{

        if (!levelUp2)
		{
			foreach (Transform child in clickedButton.transform)
			{
				if (child.name == "Tower(Clone)")
				{
					if (child.GetComponent<Tower>().currentLevel == 1)
					{
						towerSetter.LevelUp2(child.GetComponent<Tower>());
					}
				}
			}
			SpawnParticles();
			economyScript.economy -= 100;
            clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2 = true;

        }
	}

	public void LevelUp3()
	{

        if (!levelUp3)
		{
			foreach (Transform child in transform)
			{
				if (child.name == "Tower(Clone)")
				{
					if (child.GetComponent<Tower>().currentLevel == 2)
					{
						towerSetter.LevelUp3(child.GetComponent<Tower>());
					}
				}
			}
			SpawnParticles();
			economyScript.economy -= 100;
            clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3 = true;
        }
		levelUp3 = true;
	}

	public void SpawnParticles()
	{
		levelUpParticlesInstance = Instantiate(levelUpParticles, clickedButton.transform.position, Quaternion.identity);
    }
}
