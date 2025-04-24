using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;


public class SetTowerBaseInput : MonoBehaviour
{
	[SerializeField] private ParticleSystem levelUpParticles;
	private ParticleSystem levelUpParticlesInstance;

	public TowerSetter towerSetter;
	public GameObject clickedButton;
	public GameObject cam;
	public GameObject towerButton;
	public GameObject instanciateTowerButtons;
	public GameObject towerSelected;
	public GameObject towerOptions;
	public GameObject option1;
	public GameObject option2;
	public GameObject towerHabilities;

	private int pendingTowerId = 0;
	public bool spawnTower;
	bool levelUp2;
	bool levelUp3;

	public string[] names = { "CANONER", "BOPER", "LEISER" };
	public Sprite[] towerSpot;
	public RawImage image;
	public Texture towerCam;

	public Text cannonerPrice;
	public Text boperPrice;
	public Text leiserPrice;

	private int type = 0;

	Vector3 pos;

	public Text texto;
	public Text towerPrice;
	public Text statsText;

	public GameObject rangeGO;

	public bool isHoveringUpgradeButton = false;
	private bool hoverBlockedUntilExit = false;

	EconomyManager economyScript;
	StatesManager states;

	AudioManager audioManager;

	private void Update()
	{
		if (clickedButton != null)
		{
			cam.transform.position = new Vector3(clickedButton.transform.position.x, clickedButton.transform.position.y, -10);

			cam.transform.rotation = clickedButton.transform.rotation;

			spawnTower = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower;
			levelUp2 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2;
			levelUp3 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3;

			if (spawnTower == true)
			{
				foreach (Transform child in clickedButton.transform)
				{
					if (child.name == "Tower(Clone)")
					{
						Tower tower = child.GetComponent<Tower>();

						if (tower.type == 0)
						{
							option1.SetActive(true);
							option2.SetActive(true);
						}
						else if (tower.type == 1)
						{
							option1.SetActive(true);
							option2.SetActive(false);
						}
						else if (tower.type == 2)
						{
							option1.SetActive(false);
							option2.SetActive(true);
						}
					}
				}

				rangeGO.transform.position = clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().bounds.center;
				rangeGO.transform.localScale = new Vector3(clickedButton.transform.GetChild(2).GetComponent<Tower>().range, clickedButton.transform.GetChild(2).GetComponent<Tower>().range, 1) * 2; // Multipliquem per 2 per agafar diametre en comptes de radi

				if (isHoveringUpgradeButton && !hoverBlockedUntilExit)
				{
					ShowUpgradeStats();
					towerHabilities.SetActive(true);
				}
				else
				{
					ShowCurrentStats();
					towerHabilities.SetActive(false);
				}
				image.texture = towerCam;
			}
			else
			{
				ShowTowerOptions();
				cannonerPrice.text = towerSetter.towerStats[0].priceLevel_1_Type0.ToString();
				boperPrice.text = towerSetter.towerStats[1].priceLevel_1_Type0.ToString();
				leiserPrice.text = towerSetter.towerStats[2].priceLevel_1_Type0.ToString();
			}

			if (!levelUp3)
			{
				//towerButton.SetActive(true);
			}
			else
			{
				towerButton.SetActive(false);
			}

			pos = towerSelected.transform.position;
			if (!spawnTower)
			{
				instanciateTowerButtons.SetActive(true);
				towerOptions.SetActive(false);

				towerSelected.transform.position = new Vector3(pos.x, 0, pos.z);
			}
			else
			{
				instanciateTowerButtons.SetActive(false);
				towerOptions.SetActive(true);

				towerSelected.transform.position = new Vector3(pos.x, 14, pos.z);
			}
		}
	}

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	private void ShowTowerOptions()
	{
		if (pendingTowerId == -1) return;

		// Mostrar nombre
		if (pendingTowerId >= 0 && pendingTowerId < names.Length)
			texto.text = names[pendingTowerId];
		else
			texto.text = $"TOWER {pendingTowerId}";

		// Mostrar stats
		if (pendingTowerId >= 0 && pendingTowerId < towerSetter.towerStats.Count)
		{
			TowerStats stats = towerSetter.towerStats[pendingTowerId];
			statsText.text = $"Damage: {stats.damage}\nFire Rate: {stats.fireRate}\nRange: {stats.range}";

			towerPrice.text = $"{ stats.priceLevel_1_Type0}";
		}

		image.texture = towerSpot[pendingTowerId].texture;

		statsText.gameObject.SetActive(true);
		texto.gameObject.SetActive(true);
	}

	private void ShowCurrentStats()
	{
		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();

				if (tower.currentLevel == 1)
				{
                    towerPrice.text = $"{tower.priceLevel_2_Type1}";
				}
				else if (tower.currentLevel == 2)
				{
					towerPrice.text = $"{tower.priceLevel_3_Type1}";
				}
			    statsText.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}";
			}
		}
	}
	private void ShowUpgradeStats()
	{
		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();
				TowerStats upgradeStats = null;


				if (tower.currentLevel == 1)
				{
					if(tower.type == 1)
						upgradeStats = towerSetter.towerUpgrades1Type1[tower.id];
					else if (tower.type == 2)
						upgradeStats = towerSetter.towerUpgrades1Type2[tower.id];
				}
				else if (tower.currentLevel == 2)
				{
					if (tower.type == 1)
						upgradeStats = towerSetter.towerUpgrades2Type1[tower.id];
					else if (tower.type == 2)
						upgradeStats = towerSetter.towerUpgrades2Type2[tower.id];
				}
				if (upgradeStats != null)
				{
					statsText.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
									 $"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
									 $"Range: {tower.range} -> {upgradeStats.range}";
				}
			}
		}
	}

	public void OnUpgradeButtonHover(bool hover)
	{
		if (hoverBlockedUntilExit)
		{
			if (!hover)
				hoverBlockedUntilExit = false;

			isHoveringUpgradeButton = false;
			rangeGO.SetActive(false);
			return;
		}

		isHoveringUpgradeButton = hover;

		if (spawnTower == true)
		{
			bool panelOpen = transform.GetComponent<DinamicPanelAutocloser>().panel.GetComponent<Animator>().GetBool("Open");
			rangeGO.SetActive(panelOpen && hover);
		}
	}

	public void TowerButton()
	{
		hoverBlockedUntilExit = true;

		if (!spawnTower)
		{
			BuildSelectedTower();
			return;
		}

		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();
				if (tower.currentLevel == 1)
				{
					//tower.type = type;
					//LevelUp2();
					return;
				}
				else if (tower.currentLevel == 2)
				{
					LevelUp3();
				}
			}
		}
	}

	public void BuildSelectedTower()
	{
		TowerStats stats = towerSetter.towerStats[pendingTowerId];
		economyScript = FindObjectOfType<EconomyManager>();
		states = FindObjectOfType<StatesManager>();

		if (economyScript.towerSpots >= 1)
		{
			towerSetter.SpawnTower(pendingTowerId, clickedButton.transform);

			economyScript.towerSpots--;
			audioManager.PlaySFX(3, 0.2f);

			clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower = true;

			pendingTowerId = 0;
		}
		else
		{
			economyScript.insufficientTowerSpots = true;
		}
	}

	public void LevelUp2()
	{
		TowerStats stats = towerSetter.towerStats[pendingTowerId];
		if(type == 1 && !levelUp2)
		{
			if (economyScript.economy >= stats.priceLevel_2_Type1)
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
				economyScript.economy -= stats.priceLevel_2_Type1;
				audioManager.PlaySFX(4, 0.2f);
				clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2 = true;

				towerPrice.text = $"{stats.priceLevel_3_Type1}";
			}
			else
			{
				economyScript.insufficientEconomy = true;
			}
		}

		if (type == 2 && !levelUp2)
		{
			if (economyScript.economy >= stats.priceLevel_2_Type2)
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
				economyScript.economy -= stats.priceLevel_2_Type2;
				audioManager.PlaySFX(4, 0.2f);
				clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2 = true;

				towerPrice.text = $"{stats.priceLevel_3_Type2}";
			}
			else
			{
				economyScript.insufficientEconomy = true;
			}
		}

	}

	public void LevelUp3()
	{
		TowerStats stats = towerSetter.towerStats[pendingTowerId];
		if(type == 1 && !levelUp3)
		{
			if (economyScript.economy >= stats.priceLevel_3_Type1)
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
				economyScript.economy -= stats.priceLevel_3_Type1;
				audioManager.PlaySFX(4, 0.2f);
				clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3 = true;
			}
			else
			{
				economyScript.insufficientEconomy = true;
			}
		}
		if (type == 2 && !levelUp3)
		{
			if (economyScript.economy >= stats.priceLevel_3_Type2)
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
				economyScript.economy -= stats.priceLevel_3_Type2;
				audioManager.PlaySFX(4, 0.2f);
				clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3 = true;
			}
			else
			{
				economyScript.insufficientEconomy = true;
			}
		}

		levelUp3 = true;
	}
	public void SpawnCannonerTower()
	{
		if (pendingTowerId == 0)
		{
			TowerButton();
		}
		else
		{
			pendingTowerId = 0; // ID del Cannoner
		}
	}

	public void SpawnBoperTower()
	{
		if (pendingTowerId == 1)
		{
			TowerButton();
		}
		else
		{
			pendingTowerId = 1; // ID del Boper
		}
	}

	public void SpawnLeiserTower()
	{
		if (pendingTowerId == 2)
		{
			TowerButton();
		}
		else
		{
			pendingTowerId = 2; // ID del Leiser
		}
	}

	public void Type1()
	{
		type = 1;

		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();
				tower.type = type;
				LevelUp2();
			}
		}
	}

	public void Type2()
	{
		type = 2;

		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();
				tower.type = type;
				LevelUp2();
			}
		}
	}

	public void SpawnParticles()
	{
		levelUpParticlesInstance = Instantiate(levelUpParticles, clickedButton.transform.position, Quaternion.identity);
	}
}
