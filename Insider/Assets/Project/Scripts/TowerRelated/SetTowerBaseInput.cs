using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTowerBaseInput : MonoBehaviour
{
	[SerializeField] private ParticleSystem levelUpParticles;
	private ParticleSystem levelUpParticlesInstance;

	public TowerSetter towerSetter;
	public int towerGrup;

	EconomyManager economyScript;
	StatesManager states;

	bool spawnTower = false;
	bool levelUp2 = false;
	bool levelUp3 = false;

	public ControlDesplegable buttonControl;
    public void LlamarSpawnTowerOpcion1()
	{
		if (!spawnTower)
		{
			economyScript = FindObjectOfType<EconomyManager>();
			states = FindObjectOfType<StatesManager>();

			if (economyScript.economy >= 300)
			{
				buttonControl.OcultarBotonesIzquierda();

				switch (towerGrup)
				{
					case 0:
						towerSetter.SpawnTower(0, this.transform);
						break;
					case 1:
						towerSetter.SpawnTower(2, this.transform);
						break;
					case 2:
						towerSetter.SpawnTower(4, this.transform);
						break;
				}
				economyScript.economy -= 300;
			}
		}
		spawnTower = true;
	}

	public void LlamarSpawnTowerOpcion2()
	{
		if (!spawnTower)
		{
			economyScript = FindObjectOfType<EconomyManager>();
			states = FindObjectOfType<StatesManager>();
			if (economyScript.economy >= 300)
			{
				buttonControl.OcultarBotonesDerecha();


				switch (towerGrup)
				{
					case 0:
						towerSetter.SpawnTower(1, this.transform);
						break;
					case 1:
						towerSetter.SpawnTower(3, this.transform);
						break;
					case 2:
						towerSetter.SpawnTower(5, this.transform);
						break;
				}
				economyScript.economy -= 300;
			}
		}
		spawnTower = true;
	}

	public void LevelUp2()
	{
		if (!levelUp2)
		{
			foreach (Transform child in transform)
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
		}
		levelUp2 = true;
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
		}
		levelUp3 = true;
	}

	public void SpawnParticles()
	{
		levelUpParticlesInstance = Instantiate(levelUpParticles, transform.position, Quaternion.identity);
	}
}
