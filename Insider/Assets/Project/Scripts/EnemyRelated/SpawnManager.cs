using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[System.Serializable]
	public class EnemyType
	{
		public int Type1;
		public int Type2;
		public int Type3;
	}

	[System.Serializable]
	public class Wave
	{
		public string waveNumber;
		public EnemyType enemyTypes;
		public float spawnTime;
		public float delay;
	}

	[System.Serializable]
	public class GameState
	{
		public string stateName;
		public EnemyType enemyTypes;
		public List<Wave> waves;
		public float spawnTime;
		public float delay;
	}

	[System.Serializable]
	public class Root
	{
		public List<GameState> gameStates;
	}

	public List<char> enemiesList = new List<char>();
	private Root gameStateRoot;
	private int currentStateIndex = 0;
	private int currentWaveIndex = 0;

	public GameState currentState;


    public string jsonFilePath = "Assets/Project/Resources/gameStates.json";

	void Start()
	{
		LoadGameStates();
		InitializeGameState();
	}

	void LoadGameStates()
	{
		string jsonFileName = "gameStates";
		TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFileName);
		if (jsonTextAsset == null)
		{
			Debug.LogError($"No se encontró el archivo JSON en Resources con el nombre {jsonFileName}");
			return;
		}

		string jsonText = jsonTextAsset.text;
		gameStateRoot = JsonUtility.FromJson<Root>(jsonText);
	}

	void InitializeGameState()
	{
		if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
		{
			currentState = gameStateRoot.gameStates[currentStateIndex];
			if (currentState.stateName == "initial")
			{
				Debug.Log($"Estado: {currentState.stateName}");
				PopulateEnemiesList(currentState.enemyTypes);
			}
			else if (currentState.stateName == "wave")
			{
				Debug.Log($"Estado: {currentState.stateName}");
				currentWaveIndex = 0;
				InitializeWave();
			}
			else if (currentState.stateName == "finish")
			{
				Debug.Log($"Estado: {currentState.stateName}");
			}
		}
		else
		{
			Debug.Log("No hay más estados del juego.");
		}
	}

	public void AdvanceGameState()
	{
		enemiesList.Clear();

		if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
		{
			GameState currentState = gameStateRoot.gameStates[currentStateIndex];

			if (currentState.stateName == "wave" && currentWaveIndex < currentState.waves.Count - 1)
			{
				float currentTime = Time.deltaTime;
				//if(currentState.delay == )
				currentWaveIndex++;
				InitializeWave();
			}
			else
			{
				currentStateIndex++;
				currentWaveIndex = 0;
				InitializeGameState();
			}
		}
		else
		{
			Debug.Log("No hay más estados del juego.");
		}
	}

	void InitializeWave()
	{
		if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
		{
			GameState currentState = gameStateRoot.gameStates[currentStateIndex];
			if (currentWaveIndex < currentState.waves.Count)
			{
				Wave currentWave = currentState.waves[currentWaveIndex];
				Debug.Log($"Iniciando ola {currentWave.waveNumber}");
				PopulateEnemiesList(currentWave.enemyTypes);
			}
		}
	}

	void PopulateEnemiesList(EnemyType enemyType)
	{
		if (enemyType != null)
		{
			AddEnemiesToListWithProbabilities(enemyType);
		}

		Debug.Log($"Lista de enemigos generada: {string.Join(", ", enemiesList)}");
	}

	void AddEnemiesToListWithProbabilities(EnemyType enemyType)
	{
		int totalEnemiesType1 = enemyType.Type1;
		int totalEnemiesType2 = enemyType.Type2;
		int totalEnemiesType3 = enemyType.Type3;

		while (totalEnemiesType1 > 0)
		{
			float randValue = Random.value; // 0.0 - 1.0
			if (randValue < 0.8f && totalEnemiesType1 > 0)
			{
				enemiesList.Add('1');
				--totalEnemiesType1;
			}
			else if (totalEnemiesType2 > 0)
			{
				enemiesList.Add('2');
				--totalEnemiesType2;
			}
		}

		while (totalEnemiesType2 > 0)
		{
			float randValue = Random.value;
			if (randValue < 0.7f && totalEnemiesType2 > 0)
			{
				enemiesList.Add('2');
				--totalEnemiesType2;
			}
			else if (totalEnemiesType3 > 0)
			{
				enemiesList.Add('3');
				--totalEnemiesType3;
			}
		}

		while (totalEnemiesType3 > 0)
		{
			enemiesList.Add('3');
			--totalEnemiesType3;
		}
	}
}


