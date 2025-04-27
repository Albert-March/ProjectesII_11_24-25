using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPoint
{
    public Transform transformChild;
    public bool isSpawnable;
}

public class Spawner : MonoBehaviour
{

    //SPAWNING THE ENEMIES
    //Get from LevelManager
    public Queue<EnemyStats> pendingEnemies = new Queue<EnemyStats>();
    //EnemyManager vars
    public List<EnemyStats> spawneableEnemies = new List<EnemyStats>();

    private float spawnTime = 1f;
    private float currentSpawnTime = 0.0f;
	private bool isInitialized = false;
    public bool waitingForNextWave = false;
    public bool spodsRecived = false;
    private bool firstTime = true;

	public EnemyManager enemyManager;
    public TargetManager targetManager;
	private SpawnManager spawnManager;
	public EconomyManager economyManager;

	public Image buttonImage;
    public List<Sprite> sprites = new List<Sprite>();

	private Vector3 originalScale;
	public float pulseSpeed = 2f;
	public float pulseAmount = 0.1f;

	public float shakeDuration = 0.3f;
	public float shakeMagnitude = 10f;
	private Vector3 originalPosition;
	private bool isShaking = false;

    public AudioManager audioManager;

	//SELECTING THE SPAWN POINT
	public Transform SP;
    List<SpawnPoint> childs = new List<SpawnPoint>();

    public void Start()
    {
        foreach (Transform child in SP)
        {
            SpawnPoint childHolder = new SpawnPoint();
            childHolder.transformChild = child;
            childHolder.isSpawnable = true;

            childs.Add(childHolder);
        }
		spawnManager = FindObjectOfType<SpawnManager>();
		economyManager = FindObjectOfType<EconomyManager>();

		originalScale = buttonImage.transform.localScale;
		originalPosition = buttonImage.transform.localPosition;
	}

    public void Update()
    {
        if(!spawnManager.isInDelayState)
            currentSpawnTime += Time.deltaTime;

        if(currentSpawnTime > spawnTime)
        {
            SpawnEnemy();
            currentSpawnTime = 0;
        }

		if (pendingEnemies.Count == 0)
		{
            if(isInitialized == false)
            {
				UpdatePendingEnemies();
                isInitialized = true;
			}
            else
            {
                if (enemyManager.EnemiesOnScreen <= 0) 
                {
                    spawnManager.AdvanceGameState();
                    isInitialized = false;
                }
            }

		}

        if (spawnManager.currentState.stateName == "wave" && spawnManager.currentWaveIndex < spawnManager.currentState.waves.Count - 1 &&
			enemyManager.EnemiesOnScreen <= 0 && pendingEnemies.Count == 0)
        {
            waitingForNextWave = true;
			if (!spodsRecived)
			{
				if (!firstTime)
				{
					economyManager.towerSpots++;
				}
				else
				{
					firstTime = false;
				}

				spodsRecived = true;
			}
		}
		else
		{
			waitingForNextWave = false;
			spodsRecived = false;
		}

		if (waitingForNextWave)
        {
			buttonImage.sprite = sprites[0];
			float scaleFactor = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
			buttonImage.transform.localScale = originalScale * scaleFactor;
		}
        else
        {
			buttonImage.sprite = sprites[1];
			buttonImage.transform.localScale = originalScale;
		}
	}

	public void NextWave()
	{
        audioManager.PlaySFX(14, 1f);

        if (waitingForNextWave)
		{
			spawnManager.currentWaveIndex++;
			spawnManager.InitializeWave();
		}
		else
		{
			if (!isShaking)
				StartCoroutine(ShakeButton());
		}
	}

	private void UpdatePendingEnemies()
	{
		pendingEnemies.Clear();
		foreach (char c in spawnManager.enemiesList)
		{
			pendingEnemies.Enqueue(spawneableEnemies[c - '1']);
		}
	}

	private void SpawnEnemy()
    {
        if (pendingEnemies.Count != 0) 
        {
            EnemyStats enemyToSpawn = pendingEnemies.Dequeue();
            Transform spawnPoint = GetLeastCooldownChild().transformChild;
            Enemy e = Instantiate(enemyToSpawn.prefab, spawnPoint.position, Quaternion.identity, null).GetComponent<Enemy>();
            e.SetEnemyData(enemyToSpawn);
            e.enemyManager = enemyManager;
            e.path = targetManager.GetRandomPath();
            e.enabled = true;

            enemyManager.AddSpawnedEnemy(e);
        }
    }

    public SpawnPoint GetLeastCooldownChild()
    {
        int counter = 0;
        int giveChildNum = 0;
        bool allChildsSpawnable = true;

        foreach(SpawnPoint c in childs)
        {
            if (c.isSpawnable == false) 
            {
                giveChildNum = counter;
                allChildsSpawnable = false;
                break;
            }
            counter++;
        }

        if (!allChildsSpawnable) 
        {
            int randomNum = Random.Range(0, childs.Count - 1);
            if (randomNum >= giveChildNum)
            {
                childs[giveChildNum].isSpawnable = true;
                childs[randomNum + 1].isSpawnable = false;
                return childs[randomNum + 1];
            }
            else 
            {
                childs[giveChildNum].isSpawnable = true;
                childs[randomNum].isSpawnable = false;
                return childs[randomNum];
            }
            
        }


        giveChildNum = Random.Range(0, childs.Count);
        childs[giveChildNum].isSpawnable = false;

        return childs[giveChildNum];
    }

	IEnumerator ShakeButton()
	{
		isShaking = true;
		float elapsed = 0f;

		while (elapsed < shakeDuration)
		{
			float xOffset = Mathf.Sin(elapsed * 50f) * shakeMagnitude;
			buttonImage.transform.localPosition = originalPosition + new Vector3(xOffset, 0f, 0f);
			elapsed += Time.deltaTime;
			yield return null;
		}

		buttonImage.transform.localPosition = originalPosition;
		isShaking = false;
	}
}

