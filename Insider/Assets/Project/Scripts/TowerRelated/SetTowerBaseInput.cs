using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTowerBaseInput : MonoBehaviour
{
	public GameObject mainButton;
	public GameObject botonOpcion1;
	public GameObject botonOpcion2;
	public TowerSetter towerSetter;
	public int towerGrup;
	bool active = false;

	void Start()
	{
		botonOpcion1.SetActive(false);
		botonOpcion2.SetActive(false);
	}

	public void MostrarOpciones()
	{
		if(active == false)
		{
			botonOpcion1.SetActive(true);
			botonOpcion2.SetActive(true);
			active = true;
		}
		else
		{
			botonOpcion1.SetActive(false);
			botonOpcion2.SetActive(false);
			active = false;
		}	
	}

	public void LlamarSpawnTowerOpcion1()
	{
		switch(towerGrup)
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
		botonOpcion1.SetActive(false);
		botonOpcion2.SetActive(false);
		mainButton.SetActive(false);
	}

	public void LlamarSpawnTowerOpcion2()
	{
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
		botonOpcion1.SetActive(false);
		botonOpcion2.SetActive(false);
		mainButton.SetActive(false);
	}
}
