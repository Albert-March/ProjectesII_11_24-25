using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    private Root gameStateRoot;
    private int currentStateIndex = 0;
    public int currentWaveIndex = 0;

    public GameState currentState;

    public bool isInDelayState = false;
    private float time;

    public string jsonFilePath = "Assets/Project/Resources/gameStates.json";

    void Start()
    {
        LoadGameStates();
        InitializeGameState();
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
        string jsonFileName = "gameStates";
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFileName);
        if (jsonTextAsset == null)
        {
            Debug.LogError($"No se encontró el archivo JSON en Resources con el nombre {jsonFileName}");
            return;
        }

        string jsonText = jsonTextAsset.text;
        gameStateRoot = JsonUtility.FromJson<Root>(jsonText);

        if (gameStateRoot != null)
        {
            foreach (var gameState in gameStateRoot.gameStates)
            {
                Debug.Log($"Loaded GameState: {gameState.stateName}, SpawnTime: {gameState.spawnTime}, Delay: {gameState.delay}");
                if (gameState.waves != null)
                {
                    foreach (var wave in gameState.waves)
                    {
                        Debug.Log($" - Wave: {wave.waveNumber}, SpawnTime: {wave.spawnTime}, Delay: {wave.delay}");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Failed to parse the JSON into gameStateRoot.");
        }
    }

    void InitializeGameState()
    {
        if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
        {
            currentState = gameStateRoot.gameStates[currentStateIndex];
            Debug.Log($"Estado actual: {currentState.stateName}");

            if (currentState.stateName == "initial" || currentState.stateName == "finish")
            {
                PopulateEnemiesList(currentState.enemyTypes);
                StartDelay(currentState.delay);
            }
            else if (currentState.stateName == "wave")
            {
                currentWaveIndex = 0;
                InitializeWave();
            }
        }
        else
        {
            Debug.Log("No hay más estados del juego.");
        }
    }

    public void AdvanceGameState()
    {
        if (isInDelayState) return;

        enemiesList.Clear();

        if (gameStateRoot != null && currentStateIndex < gameStateRoot.gameStates.Count)
        {
            GameState currentState = gameStateRoot.gameStates[currentStateIndex];

            if (currentState.stateName == "wave" && currentWaveIndex < currentState.waves.Count - 1)
            {
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
        if (currentState.waves != null && currentWaveIndex < currentState.waves.Count)
        {
            Wave currentWave = currentState.waves[currentWaveIndex];
            currentState.delay = currentWave.delay;
            currentState.spawnTime = currentWave.spawnTime;
            Debug.Log($"Iniciando ola {currentWave.waveNumber}");
            PopulateEnemiesList(currentWave.enemyTypes);
            StartDelay(currentWave.delay);
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
        int totalEnemiesType4 = enemyType.Type4;
        int totalEnemiesType5 = enemyType.Type5;
        int totalEnemiesType6 = enemyType.Type6;
        int totalEnemiesType7 = enemyType.Type7;

        while (totalEnemiesType1 > 0)
        {
            float randValue = Random.value;
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
            float randValue = Random.value;
            if (randValue < 0.6f && totalEnemiesType3 > 0)
            {
                enemiesList.Add('3');
                --totalEnemiesType3;
            }
            else if (totalEnemiesType4 > 0)
            {
                enemiesList.Add('4');
                --totalEnemiesType4;
            }
        }

        while (totalEnemiesType4 > 0)
        {
            float randValue = Random.value;
            if (randValue < 0.5f && totalEnemiesType4 > 0)
            {
                enemiesList.Add('4');
                --totalEnemiesType4;
            }
            else if (totalEnemiesType5 > 0)
            {
                enemiesList.Add('5');
                --totalEnemiesType5;
            }
        }

        while (totalEnemiesType5 > 0)
        {
            float randValue = Random.value;
            if (randValue < 0.4f && totalEnemiesType5 > 0)
            {
                enemiesList.Add('5');
                --totalEnemiesType5;
            }
            else if (totalEnemiesType6 > 0)
            {
                enemiesList.Add('6');
                --totalEnemiesType6;
            }
        }

        while (totalEnemiesType6 > 0)
        {
            float randValue = Random.value;
            if (randValue < 0.3f && totalEnemiesType6 > 0)
            {
                enemiesList.Add('6');
                --totalEnemiesType6;
            }
            else if (totalEnemiesType7 > 0)
            {
                enemiesList.Add('7');
                --totalEnemiesType7;
            }
        }

        while (totalEnemiesType7 > 0)
        {
            enemiesList.Add('7');
            --totalEnemiesType7;
        }
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
            Debug.Log("Delay terminado.");
        }
    }
    
}


