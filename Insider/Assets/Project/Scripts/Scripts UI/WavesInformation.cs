using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class WavesInformation : MonoBehaviour
{
	public Spawner spawner;
	public SpawnManager spawnManager;

	public List<char> simulatedEnemies;

	public List<GameObject> enemyInfoObjects = new List<GameObject>(); // Tus 5 GameObjects

	public List<Sprite> nextWaveEnemySprites = new List<Sprite>();

	public string[] names = { "Normal", "Splitter", "Runner", "Healer", "Tank"};

	private bool infoShown = false;

	private Animator panelAnimator;
	public bool isPanelOpen = false;
	private bool lastPanelState = false;
	private bool autocloser = false;

	private void Start()
	{
		ClearUI();

		panelAnimator = GetComponent<Animator>();
	}
	void Update()
	{
		if (spawner.waitingForNextWave && !infoShown)
		{
			ShowNextWaveEnemyInfo();
			infoShown = true;
		}
		else if (!spawner.waitingForNextWave)
		{
			infoShown = false;
		}

		PrintRemainingEnemies();

		//Panel
		if (panelAnimator != null && isPanelOpen != lastPanelState)
		{
			panelAnimator.SetBool("Open", isPanelOpen);
			lastPanelState = isPanelOpen;
		}
		if(!spawner.waitingForNextWave && !autocloser)
		{
			isPanelOpen = false;
			autocloser = true;
		}
	}

	void ShowNextWaveEnemyInfo()
	{
		ClearUI();

		if (spawnManager.currentState.stateName != "wave") return;
		if (spawnManager.currentWaveIndex + 1 >= spawnManager.currentState.waves.Count) return;

		var nextWave = spawnManager.currentState.waves[spawnManager.currentWaveIndex + 1];
		simulatedEnemies = spawnManager.GetSimulatedEnemiesList(nextWave.enemyTypes);
		Debug.Log("Enemigos siguiente oleada: " + string.Join(", ", simulatedEnemies));

		isPanelOpen = true;
		autocloser = false;
	}

	void PrintRemainingEnemies()
	{
		if (simulatedEnemies == null || simulatedEnemies.Count == 0)
		{
			Debug.Log("No quedan enemigos en la oleada actual.");
			ClearUI();
			return;
		}

		Debug.Log("Enemigos restantes: " + string.Join(", ", simulatedEnemies));

		// Agrupar enemigos por tipo
		var agrupados = new Dictionary<char, int>();
		foreach (var c in simulatedEnemies)
		{
			if (!agrupados.ContainsKey(c))
				agrupados[c] = 0;
			agrupados[c]++;
		}

		var tipos = new List<char>(agrupados.Keys);
		tipos.Sort();

		for (int i = 0; i < enemyInfoObjects.Count; i++)
		{
			if (i < tipos.Count)
			{
				var obj = enemyInfoObjects[i];
				char tipoChar = tipos[i];
				int tipo = tipoChar - '0';
				int cantidad = agrupados[tipoChar];

				Image img = obj.GetComponentInChildren<Image>();
				Text txt = obj.GetComponentInChildren<Text>();

				txt.text = $"{names[tipo - 1]}: {cantidad}";

				if (tipo - 1 < nextWaveEnemySprites.Count)
				{
					img.sprite = nextWaveEnemySprites[tipo - 1];
					img.enabled = true;
				}
				else
				{
					img.enabled = false;
				}

				obj.SetActive(true);
			}
			else
			{
				enemyInfoObjects[i].SetActive(false);
			}
		}
	}

	void ClearUI()
	{
		foreach (var obj in enemyInfoObjects)
		{
			obj.SetActive(false);
		}
	}

	public void TogglePanel()
	{
		isPanelOpen = !isPanelOpen;
	}
}
