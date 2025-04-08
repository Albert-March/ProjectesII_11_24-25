using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
	[System.Serializable]
	public class EnemyType
	{
		public int Type1;
		public int Type2;
		public int Type3;
		public int Type4;
		public int Type5;
		public int Type6;
		public int Type7;
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
	public List<int> pathIndicesList = new List<int>();
	private Root gameStateRoot;
	private int currentStateIndex = 0;
	public int currentWaveIndex = 0;

	public GameState currentState;

	public bool isInDelayState = false;
	private bool pendingStateAdvance = false;
	private float time;

	string jsonFileName;
	string sceneName;

	void Start()
	{
		sceneName = SceneManager.GetActiveScene().name;
		LoadGameStates();

		if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
		{
			currentState = gameStateRoot.gameStates[currentStateIndex];
		}
	}

	void Update()
	{
		if (isInDelayState)
		{
			StayOnDelay();
		}
	}

	void LoadGameStates()
	{
		switch (sceneName)
		{
			case "Level_1":
				jsonFileName = "Level_1_States";
				break;
			case "Level_2":
				jsonFileName = "Level_2_States";
				break;
			default:
				break;
		}

		TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFileName);
		if (jsonTextAsset == null)
			return;

		string jsonText = jsonTextAsset.text;
		gameStateRoot = JsonUtility.FromJson<Root>(jsonText);
	}

	public void AdvanceGameState()
	{
		if (isInDelayState) return;

		enemiesList.Clear();

		if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
		{
			if (currentState.stateName == "initial")
			{
				StartDelay(currentState.delay);
				pendingStateAdvance = true;
			}
			else if (currentState.stateName == "wave" && currentWaveIndex > currentState.waves.Count - 1)
			{
				StartDelay(currentState.delay);
				pendingStateAdvance = true;
			}
		}
	}

	public void InitializeWave()
	{
		if (currentState.waves != null && currentWaveIndex < currentState.waves.Count)
		{
			Wave currentWave = currentState.waves[currentWaveIndex];
			currentState.delay = currentWave.delay;
			currentState.spawnTime = currentWave.spawnTime;
			PopulateEnemiesList(currentWave.enemyTypes);
			StartDelay(currentWave.delay);
		}
	}

	void PopulateEnemiesList(EnemyType enemyType)
	{
		if (enemyType != null)
		{
			AddEnemiesInDeterministicOrder(enemyType);
		}
	}

	void AddEnemiesInDeterministicOrder(EnemyType enemyType)
	{
		Dictionary<int, int> enemigosPorTipo = new Dictionary<int, int>
	{
		{ 1, enemyType.Type1 },
		{ 2, enemyType.Type2 },
		{ 3, enemyType.Type3 },
		{ 4, enemyType.Type4 },
		{ 5, enemyType.Type5 }
	};

		List<int> secuencia = GenerarSecuenciaEnemigos(enemigosPorTipo);

		enemiesList.Clear();
		foreach (int tipo in secuencia)
		{
			enemiesList.Add(tipo.ToString()[0]);
		}

		// Generar caminos deterministas basados en la secuencia
		pathIndicesList.Clear();
		string seedString = string.Join("", enemiesList);
		int seed = seedString.GetHashCode();

		System.Random rng = new System.Random(seed);
		int pathCount = FindObjectOfType<TargetManager>().targetLists.Count;

		for (int i = 0; i < enemiesList.Count; i++)
		{
			int pathIndex = rng.Next(pathCount);
			pathIndicesList.Add(pathIndex);
		}
	}

	public List<char> GetSimulatedEnemiesList(EnemyType enemyType)
	{
		Dictionary<int, int> enemigosPorTipo = new Dictionary<int, int>
	{
		{ 1, enemyType.Type1 },
		{ 2, enemyType.Type2 },
		{ 3, enemyType.Type3 },
		{ 4, enemyType.Type4 },
		{ 5, enemyType.Type5 }
	};

		List<int> secuencia = GenerarSecuenciaEnemigos(enemigosPorTipo);

		List<char> simulatedList = new List<char>();
		foreach (int tipo in secuencia)
		{
			simulatedList.Add(tipo.ToString()[0]);
		}

		return simulatedList;
	}

	List<int> GenerarSecuenciaEnemigos(Dictionary<int, int> enemigosPorTipo)
	{
		// Crear una lista con todos los enemigos según cantidad
		List<int> allEnemies = new List<int>();
		foreach (var kvp in enemigosPorTipo.OrderBy(k => k.Key)) // orden por tipo (ID)
		{
			for (int i = 0; i < kvp.Value; i++)
			{
				allEnemies.Add(kvp.Key);
			}
		}

		// Distribuirlos uniformemente según su tipo (los altos más al final)
		List<int> resultado = new List<int>(new int[allEnemies.Count]);
		List<int> disponibles = new List<int>(allEnemies);
		Dictionary<int, float> posiciones = new Dictionary<int, float>();
		Dictionary<int, int> colocados = new Dictionary<int, int>();

		int total = disponibles.Count;

		// Inicializamos la posición objetivo para cada tipo
		var tiposOrdenados = enemigosPorTipo.Keys.OrderBy(k => k).ToList();
		foreach (var tipo in tiposOrdenados)
		{
			if (enemigosPorTipo[tipo] > 0)
			{
				posiciones[tipo] = (float)tiposOrdenados.IndexOf(tipo) / tiposOrdenados.Count;
				colocados[tipo] = 0;
			}
		}

		for (int i = 0; i < total; i++)
		{
			float mejorPuntaje = float.MinValue;
			int mejorTipo = -1;

			foreach (var tipo in tiposOrdenados)
			{
				if (!colocados.ContainsKey(tipo) || colocados[tipo] >= enemigosPorTipo[tipo])
					continue;

				float idealPos = posiciones[tipo] * total;
				float progreso = (float)colocados[tipo] / enemigosPorTipo[tipo];
				float actualPos = progreso * total;

				float puntaje = -Mathf.Abs(idealPos - i); // preferimos ponerlo donde más cerca esté de su "target"

				if (puntaje > mejorPuntaje)
				{
					mejorPuntaje = puntaje;
					mejorTipo = tipo;
				}
			}

			resultado[i] = mejorTipo;
			colocados[mejorTipo]++;
		}

		return resultado;
	}

	void StartDelay(float delayDuration)
	{
		isInDelayState = true;
		time = 0;
		currentState.delay = delayDuration;
	}

	public void StayOnDelay()
	{
		if (time < currentState.delay)
		{
			time += Time.deltaTime;
		}
		else
		{
			isInDelayState = false;
			time = 0;

			if (pendingStateAdvance)
			{
				currentStateIndex++;
				if (currentStateIndex < gameStateRoot.gameStates.Count)
				{
					currentState = gameStateRoot.gameStates[currentStateIndex];

					if (currentState.stateName == "wave")
					{
						currentWaveIndex = -1; // si quieres esperar botón para avanzar
					}
				}
				pendingStateAdvance = false;
			}
		}
	}
}


