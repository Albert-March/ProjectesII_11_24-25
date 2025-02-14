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

	public string[] names = { "LEISER", "CHOMPER", "BOMBER", "CANONER", "TAGER", "BOPER" };

	public Text textoA;
	public Text textoB;
	public Text price;

	public GameObject rangeGO;


    EconomyManager economyScript;
	StatesManager states;

	AudioManager audioManager;
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

            if (spawnTower == true)
            {
                rangeGO.transform.position = new Vector2(clickedButton.transform.position.x, clickedButton.transform.position.y) - clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().offset;
				rangeGO.transform.loca = clickedButton.transform.rotation;
                rangeGO.transform.localScale = new Vector3(clickedButton.transform.GetChild(2).GetComponent<Tower>().range, clickedButton.transform.GetChild(2).GetComponent<Tower>().range, 1) * 2;
            }

            switch (towerGrup)
			{
				case 0:
					textoA.text = names[0];
					textoB.text = names[1];
                    break;
				case 1:
                    textoA.text = names[2];
                    textoB.text = names[3];
                    break;
				case 2:
                    textoA.text = names[4];
                    textoB.text = names[5];
                    break;
            }

			if (levelUp3 == true)
			{ price.text = " "; }
			else if (levelUp2 == true)
			{ price.text = "800"; }
			else if (spawnTower == true)
			{ price.text = "400"; }
			else 
			{ price.text = "200"; }
        }
    }
	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	public void LlamarSpawnTowerOpcion1()
	{
        
        if (!spawnTower)
		{
			economyScript = FindObjectOfType<EconomyManager>();
			states = FindObjectOfType<StatesManager>();

			if (economyScript.economy >= 200)
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
				economyScript.economy -= 200;
				audioManager.PlaySFX(3, 0.2f);
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
			if (economyScript.economy >= 200)
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
				economyScript.economy -= 200;
				audioManager.PlaySFX(3, 0.2f);
				clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower = true;
            }
		}
	}

	public void LevelUp2()
	{

        if (!levelUp2 && economyScript.economy >= 400)
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
			economyScript.economy -= 400;
			audioManager.PlaySFX(4, 0.2f);
			clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2 = true;

        }
	}

	public void LevelUp3()
	{

        if (!levelUp3 && economyScript.economy >= 800)
		{
			foreach (Transform child in clickedButton.transform)
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
			economyScript.economy -= 800;
			audioManager.PlaySFX(4, 0.2f);
			clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3 = true;
        }
		levelUp3 = true;
	}

	public void SpawnParticles()
	{
		levelUpParticlesInstance = Instantiate(levelUpParticles, clickedButton.transform.position, Quaternion.identity);
    }
}
