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
	public GameObject Op1;
	public GameObject Op1_1;
	public GameObject option2;
	public GameObject Op2;
	public GameObject Op2_1;

	public Text Price1;
	public Text Price1_1;
	public Text Price2;
	public Text Price2_1;

	public Animator animator;

	public GameObject materialL;
	public GameObject materialR;
	public Material material;
	private Coroutine lerpCoroutine;

	private Vector3 option1OriginalScale;
	private Vector3 option2OriginalScale;
	public float pulseSpeed = 2f;
	public float pulseAmount = 0.1f;

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
	private bool posibleType1 = false;
	private bool posibleType1_1 = false;
	private bool posibleType2 = false;
	private bool posibleType2_1 = false;

	private bool justUpdated = false;

	public Text texto;
	public Text towerPrice;
	public Text statsText;

	public GameObject rangeGO;

	public bool isHoveringUpgradeButton = false;
	private bool hoverBlockedUntilExit = false;

	EconomyManager economyScript;
	StatesManager states;
	PanelVisibilityController panelVisibilityController;

	AudioManager audioManager;
	void Start() 
	{
		option1OriginalScale = Op1.transform.localScale;
		//option1OriginalScale = Op1_1.transform.localScale;
		option2OriginalScale = Op2.transform.localScale;
		//option2OriginalScale = Op2_1.transform.localScale;
	}
	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		panelVisibilityController = GetComponent<PanelVisibilityController>();
	}

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
						float scaleFactor = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

						UpdateMaterialBasedOnTower(tower);

						if (tower.type == 0)
						{
							option1.SetActive(true);
							Op1.transform.localScale = option1OriginalScale * scaleFactor;

							option2.SetActive(true);
							Op2.transform.localScale = option2OriginalScale * scaleFactor;

							//material.SetFloat("_AmountDisplayed", 1f);
							materialL.SetActive(true);
							materialR.SetActive(true);
						}
						else if (tower.type == 1)
						{
							option1.SetActive(true);
							option2.SetActive(false);

							if (tower.currentLevel == 2)
							{
								Op1_1.transform.localScale = option1OriginalScale * scaleFactor;
							}

							materialL.SetActive(true);
							materialR.SetActive(false);
						}
						else if (tower.type == 2)
						{
							option1.SetActive(false);
							option2.SetActive(true);

							if (tower.currentLevel == 2)
							{
								Op2_1.transform.localScale = option1OriginalScale * scaleFactor;
							}

							materialL.SetActive(false);
							materialR.SetActive(true);
						}
						Price1.text = tower.priceLevel_2_Type1.ToString();
						Price1_1.text = tower.priceLevel_2_Type2.ToString();
						Price2.text = tower.priceLevel_3_Type1.ToString();
						Price2_1.text = tower.priceLevel_3_Type2.ToString();


						if (isHoveringUpgradeButton && !hoverBlockedUntilExit && !justUpdated)
						{
							if (((posibleType1 || posibleType2) && tower.currentLevel == 1) || ((posibleType1_1 || posibleType2_1) && tower.currentLevel == 2))
							{
								ShowUpgradeStats();
							}
						}
						else
						{
							ShowCurrentStats();
						}
						image.texture = towerCam;
					}
				}
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

			if (!spawnTower)
			{
				instanciateTowerButtons.SetActive(true);
				towerOptions.SetActive(false);

				animator.SetBool("Open", false);
			}
			else
			{
				instanciateTowerButtons.SetActive(false);
				towerOptions.SetActive(true);

				animator.SetBool("Open", true);
			}
		}

		if(panelVisibilityController.open && !posibleType1 && !posibleType1_1 && !posibleType2 && !posibleType2_1)
		{
			foreach (Transform child in clickedButton.transform)
			{
				if (child.name == "Tower(Clone)")
				{
					rangeGO.transform.position = clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().bounds.center;
					rangeGO.transform.localScale = new Vector3(clickedButton.transform.GetChild(2).GetComponent<Tower>().range, clickedButton.transform.GetChild(2).GetComponent<Tower>().range, 1) * 2; // Multipliquem per 2 per agafar diametre en comptes de radi
				}
			}
		}
		bool panelOpen = transform.GetComponent<DinamicPanelAutocloser>().panel.GetComponent<Animator>().GetBool("Open");
		rangeGO.SetActive(panelOpen && panelVisibilityController.open && spawnTower);

		if (justUpdated && !posibleType1 && !posibleType1_1 && !posibleType2 && !posibleType2_1)
		{
			justUpdated = false;
		}
	}
	private void UpdateMaterialBasedOnTower(Tower tower)
	{
		if (lerpCoroutine != null)
		{
			return;
		}
		if (tower.type == 0)
		{
			material.SetFloat("_AmountDisplayed", 1f);
		}
		else if (tower.type == 1 || tower.type == 2)
		{
			if (tower.currentLevel == 2)
			{
				material.SetFloat("_AmountDisplayed", 0.5f);
			}
			else if (tower.currentLevel == 3)
			{
				material.SetFloat("_AmountDisplayed", 0f);
			}
		}
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

			if(stats.id == 0)
			{
				statsText.text = $"Damage: {stats.damage}\nFire Rate: {stats.fireRate}\nRange: {stats.range}\nBulletSpeed: {stats.projectileSpeed}";
			}
			else if(stats.id == 2 && stats.type == 1)
			{
				statsText.text = $"Damage: {stats.damage}\nFire Rate: {stats.fireRate}\nRange: {stats.range}\nTargetAmount: {stats.targetAmount}";
			}
			else
			{
				statsText.text = $"Damage: {stats.damage}\nFire Rate: {stats.fireRate}\nRange: {stats.range}";
			}

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
				if (tower.id == 0)
				{
					statsText.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}\nBulletSpeed: {tower.projectileSpeed}";
				}
				else if (tower.id == 2 && tower.type == 1)
				{
					statsText.text = $"Damage: {tower.damage}\nFire Rate: {tower.fireRate}\nRange: {tower.range}\nTargetAmount: {tower.targetAmount}";
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
					if (posibleType1)
						upgradeStats = towerSetter.towerUpgrades1Type1[tower.id];
					else if (posibleType2)
						upgradeStats = towerSetter.towerUpgrades1Type2[tower.id];
					else
						upgradeStats = towerSetter.towerStats[tower.id];
				}
				else if (tower.currentLevel == 2)
				{
					if (tower.type == 1)
						upgradeStats = towerSetter.towerUpgrades2Type1[tower.id];
					else if (tower.type == 2)
						upgradeStats = towerSetter.towerUpgrades2Type2[tower.id];
					else
						upgradeStats = towerSetter.towerStats[tower.id];
				}

				if (upgradeStats != null)
				{
					if (tower.id == 0)
					{
						statsText.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
											$"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
											$"Range: {tower.range} -> {upgradeStats.range}\n" +
											$"BulletSpeed: {tower.projectileSpeed} -> {upgradeStats.projectileSpeed}";
					}
					else if (tower.id == 2 && posibleType1 || posibleType1_1)
					{
						statsText.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
											$"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
											$"Range: {tower.range} -> {upgradeStats.range}\n" +
											$"TargetAmount: {tower.targetAmount} -> {upgradeStats.targetAmount}";
					}
					else
					{
						statsText.text = $"Damage: {tower.damage} -> {upgradeStats.damage}\n" +
											$"Fire Rate: {tower.fireRate} -> {upgradeStats.fireRate}\n" +
											$"Range: {tower.range} -> {upgradeStats.range}";
					}
				}
				rangeGO.transform.position = clickedButton.transform.GetChild(2).GetComponent<CircleCollider2D>().bounds.center;
				rangeGO.transform.localScale = new Vector3(upgradeStats.range, upgradeStats.range, 1) * 2; // Multipliquem per 2 per agafar diametre en comptes de radi
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
			//rangeGO.SetActive(false);
			return;
		}

		isHoveringUpgradeButton = hover;

		//if (spawnTower == true)
		//{
		//	bool panelOpen = transform.GetComponent<DinamicPanelAutocloser>().panel.GetComponent<Animator>().GetBool("Open");
		//	rangeGO.SetActive(panelOpen && hover);
		//}
	}

	public void TowerButton()
	{
		hoverBlockedUntilExit = true;

		if (!spawnTower)
		{
			BuildSelectedTower();
			hoverBlockedUntilExit = false;
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
					hoverBlockedUntilExit = false;
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

		justUpdated = true;
		if (lerpCoroutine != null)
		{
			StopCoroutine(lerpCoroutine);
		}
		lerpCoroutine = StartCoroutine(LerpMaterialValue(1f, 0.5f, 2f));
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

		justUpdated = true;
		if (lerpCoroutine != null)
		{
			StopCoroutine(lerpCoroutine);
		}
		lerpCoroutine = StartCoroutine(LerpMaterialValue(0.5f, 0f, 2f));
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

	public void PosibleType1(bool type)
	{
		posibleType1 = type;
	}
	public void PosibleType1_1(bool type)
	{
		posibleType1_1 = type;
	}

	public void PosibleType2(bool type)
	{
		posibleType2 = type;
	}
	public void PosibleType2_1(bool type)
	{
		posibleType2_1 = type;
	}

	public void SpawnParticles()
	{
		levelUpParticlesInstance = Instantiate(levelUpParticles, clickedButton.transform.position, Quaternion.identity);
	}
	private IEnumerator LerpMaterialValue(float startValue, float endValue, float duration)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			float value = Mathf.Lerp(startValue, endValue, t);

			material.SetFloat("_AmountDisplayed", value);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		material.SetFloat("_AmountDisplayed", endValue);
		lerpCoroutine = null;
	}
}
