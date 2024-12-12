using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class SetTowerBaseInput : MonoBehaviour
{
	public TowerSetter towerSetter;
	public int towerGrup;

	EconomyManager economyScript;
	StatesManager states;

	public ControlDesplegable buttonControl;
    public void LlamarSpawnTowerOpcion1()
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
			states.IncrementPosition();
		}
	}

	public void LlamarSpawnTowerOpcion2()
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
			states.IncrementPosition();
		}
	}
}
