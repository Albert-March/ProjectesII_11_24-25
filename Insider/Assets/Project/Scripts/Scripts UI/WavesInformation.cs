using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesInformation : MonoBehaviour
{
	public Spawner spawner;
	public SpawnManager spawnManager;

	public List<GameObject> enemyInfoObjects = new List<GameObject>(); // Tus 5 GameObjects

	public List<Sprite> nextWaveEnemySprites = new List<Sprite>();

	private bool infoShown = false;

	private void Start()
	{
		ClearUI();
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
	}

	void ShowNextWaveEnemyInfo()
	{
		ClearUI();

		if (spawnManager.currentState.stateName != "wave") return;
		if (spawnManager.currentWaveIndex + 1 >= spawnManager.currentState.waves.Count) return;

		SpawnManager.Wave nextWave = spawnManager.currentState.waves[spawnManager.currentWaveIndex + 1];
		SpawnManager.EnemyType enemyTypes = nextWave.enemyTypes;

		List<(int tipo, int cantidad)> tiposConCantidad = new List<(int, int)>
		{
			(1, enemyTypes.Type1),
			(2, enemyTypes.Type2),
			(3, enemyTypes.Type3),
			(4, enemyTypes.Type4),
			(5, enemyTypes.Type5)
		};

		List<(int tipo, int cantidad)> activos = tiposConCantidad.FindAll(t => t.cantidad > 0);

		for (int i = 0; i < enemyInfoObjects.Count; i++)
		{
			if (i < activos.Count)
			{
				var obj = enemyInfoObjects[i];
				var tipo = activos[i].tipo;
				var cantidad = activos[i].cantidad;

				Image img = obj.GetComponentInChildren<Image>();
				Text txt = obj.GetComponentInChildren<Text>();

				txt.text = $"Tipo {tipo}: {cantidad}";

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
}
