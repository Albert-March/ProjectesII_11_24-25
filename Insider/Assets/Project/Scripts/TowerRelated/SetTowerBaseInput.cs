using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class SetTowerBaseInput : MonoBehaviour
{
	[SerializeField] private ParticleSystem levelUpParticles;
	private ParticleSystem levelUpParticlesInstance;

	public TowerSetter towerSetter;
	public GameObject clickedButton;
	public GameObject cam;
	public GameObject buildTowerButton;

	private int pendingTowerId = 3;
	public bool spawnTower;
	bool levelUp2;
	bool levelUp3;

	public string[] names = { "LEISER", "CHOMPER", "BOMBER", "CANONER", "TAGER", "BOPER" };
	public Sprite[] towerSpot;
	public RawImage image;
	public Texture towerCam;

	public Text texto;
	public Text price;
	public Text statsText;

	public GameObject rangeGO;

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

			//towerGrup = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().towerId;
			spawnTower = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower;
			levelUp2 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2;
			levelUp3 = clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3;

			if (spawnTower == true)
			{
				rangeGO.transform.position = clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().bounds.center;
				rangeGO.transform.localScale = new Vector3(clickedButton.transform.GetChild(2).GetComponent<Tower>().range, clickedButton.transform.GetChild(2).GetComponent<Tower>().range, 1) * 2; // Multipliquem per 2 per agafar diametre en comptes de radi
				if (isHoveringUpgradeButton)
				{
					ShowUpgradeStats();
				}
				else
				{
					ShowCurrentStats();
				}
				image.texture = towerCam;
				buildTowerButton.SetActive(false);
			}
			else
			{
				ShowTowerOptions();
				buildTowerButton.SetActive(true);
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
		}

		image.texture = towerSpot[pendingTowerId].texture;

		statsText.gameObject.SetActive(true);
		texto.gameObject.SetActive(true);

		// Mostrar precio
		//if (levelUp3)
		//	price.text = " ";
		//else if (levelUp2)
		//	price.text = "800";
		//else if (spawnTower)
		//	price.text = "400";
		//else
		//	price.text = "200";
	}

	private void ShowCurrentStats()
	{
		foreach (Transform child in clickedButton.transform)
		{
			if (child.name == "Tower(Clone)")
			{
				Tower tower = child.GetComponent<Tower>();

				if (isHoveringUpgradeButton)
				{
					TowerStats upgradeStats = null;

					if (tower.currentLevel == 1)
						upgradeStats = towerSetter.towerUpgrades1[tower.id];
					else if (tower.currentLevel == 2)
						upgradeStats = towerSetter.towerUpgrades2[tower.id];

					if (upgradeStats != null)
					{
						statsText.text = $"Damage: {tower.damage} ? {upgradeStats.damage}\n" +
										 $"Fire Rate: {tower.fireRate} ? {upgradeStats.fireRate}\n" +
										 $"Range: {tower.range} ? {upgradeStats.range}";
					}
					else
					{
						statsText.text = $"Damage: {tower.damage}\n" +
										 $"Fire Rate: {tower.fireRate}\n" +
										 $"Range: {tower.range}\n" +
										 $"(Max Level Reached)";
					}
				}
				else
				{
					statsText.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}";
				}
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
					upgradeStats = towerSetter.towerUpgrades1[tower.id];
				}
				else if (tower.currentLevel == 2)
				{
					upgradeStats = towerSetter.towerUpgrades2[tower.id];
				}

				if (upgradeStats != null)
				{
					statsText.text = $"Damage: {tower.damage} ? {upgradeStats.damage}\n" +
									  $"Fire Rate: {tower.fireRate} ? {upgradeStats.fireRate}\n" +
									  $"Range: {tower.range} ? {upgradeStats.range}";
				}
				else
				{
					statsText.text = $"Damage: {tower.damage}\n" +
									  $"Fire Rate: {tower.fireRate}\n" +
									  $"Range: {tower.range}\n" +
									  $"(Max Level Reached)";
				}
			}
		}
	}

	public void OnUpgradeButtonHover(bool hover)
	{
		this.isHoveringUpgradeButton = hover;
		if (spawnTower == true)
			rangeGO.SetActive(transform.GetComponent<DinamicPanelAutocloser>().panel.GetComponent<Animator>().GetBool("Open") && hover);

	}

	public void BuildSelectedTower()
	{
		economyScript = FindObjectOfType<EconomyManager>();
		states = FindObjectOfType<StatesManager>();

		if (economyScript.economy >= 200)
		{
			towerSetter.SpawnTower(pendingTowerId, clickedButton.transform);

			economyScript.economy -= 200;
			audioManager.PlaySFX(3, 0.2f);

			clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower = true;

			pendingTowerId = 3;
		}
	}

	public void SpawnCannonerTower()
	{
		pendingTowerId = 3; // ID del Cannoner
	}

	public void SpawnBomberTower()
	{
		pendingTowerId = 2; // ID del Bomber
	}

	public void SpawnTagerTower()
	{
		pendingTowerId = 4; // ID del Tager
	}

	public void SpawnBoperTower()
	{
		pendingTowerId = 5; // ID del Boper
	}

	public void SpawnLeiserTower()
	{
		pendingTowerId = 0; // ID del Leiser
	}

	public void SpawnChomperTower()
	{
		pendingTowerId = 1; // ID del Chomper
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
