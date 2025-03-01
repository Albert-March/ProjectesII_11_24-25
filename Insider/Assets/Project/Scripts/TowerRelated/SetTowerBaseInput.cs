using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class SetTowerBaseInput : MonoBehaviour
{
	[SerializeField] private ParticleSystem levelUpParticles;
	private ParticleSystem levelUpParticlesInstance;

	public GameObject buttonTowerA;
	public GameObject buttonTowerB;

	public TowerSetter towerSetter;
	public GameObject clickedButton;
	public GameObject cam;

    public int towerGrup;
    public bool spawnTower;
	bool levelUp2;
	bool levelUp3;

	public string[] names = { "LEISER", "CHOMPER", "BOMBER", "CANONER", "TAGER", "BOPER" };
	private char type = 'A';

	public Text textoA;
	public Text textoB;
	public Text priceA;
	public Text priceB;

	public GameObject rangeGO;

	public Text statsTextA;
	public Text statsTextB;
	public bool isHoveringUpgradeButton = false;


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
                rangeGO.SetActive(transform.GetComponent<DinamicPanelAutocloser>().panel.GetComponent<Animator>().GetBool("Open"));
                rangeGO.transform.position = clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().bounds.center;
				rangeGO.transform.localScale = new Vector3(clickedButton.transform.GetChild(2).GetComponent<Tower>().range, clickedButton.transform.GetChild(2).GetComponent<Tower>().range, 1) * 2; // Multipliquem per 2 per agafar diametre en comptes de radi

                Debug.Log(isHoveringUpgradeButton);
                if (isHoveringUpgradeButton)
				{
					ShowUpgradeStats();
				}
				else
				{
					ShowCurrentStats();
				}
			}
			else
			{
				ShowTowerOptions();
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
			{ priceA.text = " "; priceB.text = " "; }
			else if (levelUp2 == true)
			{ priceA.text = "800"; priceB.text = "800"; }
			else if (spawnTower == true)
			{ priceA.text = "400"; priceB.text = "400"; }
			else
			{ priceA.text = "200"; priceB.text = "200"; }
		}
    }
	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	private void ShowTowerOptions()
	{
		TowerStats option1 = towerSetter.towerStats[towerGrup * 2];
		TowerStats option2 = towerSetter.towerStats[towerGrup * 2 + 1];
        statsTextA.gameObject.SetActive(true);
        statsTextB.gameObject.SetActive(true);
        textoA.gameObject.SetActive(true);
        textoB.gameObject.SetActive(true);
		buttonTowerA.gameObject.SetActive(true);
		buttonTowerB.gameObject.SetActive(true);
        statsTextA.text = $"Damage: {option1.damage}\nFire Rate: {option1.fireRate}\nRange: {option1.range}";
		statsTextB.text = $"Damage: {option2.damage}\nFire Rate: {option2.fireRate}\nRange: {option2.range}";
	}
	private void ShowUpgradeStats()
	{
		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();
                if (type == 'A')
                {
                    if (tower.currentLevel == 1)
                    {
                        statsTextA.text = $"Damage:";
                        TowerStats upgradeStats = towerSetter.towerUpgrades1[tower.id];
                        statsTextA.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
                                          $"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
                                          $"Range: {tower.range} -> {upgradeStats.range}";
                    }
                    else if (tower.currentLevel == 2)
                    {
                        TowerStats upgradeStats = towerSetter.towerUpgrades2[tower.id];
                        statsTextA.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
                                          $"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
                                          $"Range: {tower.range} -> {upgradeStats.range}";
                    }
                    else
                    {
                        statsTextA.text = $"Damage: {tower.damage}\n" +
                                          $"Fire Rate: {tower.fireRate}\n" +
                                          $"Range: {tower.range}\n" +
                                          $"(Max Level Reached)";
                    }
                }
                else if (type == 'B')
                {
                    if (tower.currentLevel == 1)
                    {
                        Debug.Log("Prova");
                        statsTextB.text = $"Damage:";
                        TowerStats upgradeStats = towerSetter.towerUpgrades1[tower.id];
                        statsTextB.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
                                          $"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
                                          $"Range: {tower.range} -> {upgradeStats.range}";
                    }
                    else if (tower.currentLevel == 2)
                    {
                        TowerStats upgradeStats = towerSetter.towerUpgrades2[tower.id];
                        statsTextB.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
                                          $"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
                                          $"Range: {tower.range} -> {upgradeStats.range}";
                    }
                    else
                    {
                        statsTextB.text = $"Damage: {tower.damage}\n" +
                                          $"Fire Rate: {tower.fireRate}\n" +
                                          $"Range: {tower.range}\n" +
                                          $"(Max Level Reached)";
                    }
                }

			}
		}
	}
	private void ShowCurrentStats()
	{
		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();

				if (type == 'A')
				{
					statsTextA.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}";
				}
				else if (type == 'B')
				{
					statsTextB.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}";
				}
			}
		}
	}


	public void OnUpgradeButtonHover(bool hover)
	{
		Debug.Log(hover);
        this.isHoveringUpgradeButton = hover;

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
		type = 'A';
		textoB.gameObject.SetActive(false);
		statsTextB.gameObject.SetActive(false);
		buttonTowerB.gameObject.SetActive(false);
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
		type = 'B';
		textoA.gameObject.SetActive(false);
		statsTextA.gameObject.SetActive(false);
		buttonTowerA.gameObject.SetActive(false);
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
