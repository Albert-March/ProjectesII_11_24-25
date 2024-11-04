using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float spawnTime = 1.0f;
    private float currentSpawnTime = 0.0f;

    public EnemyManager enemyManager;


    //SELECTING THE SPAWN POINT
    public Transform SP;
    List<SpawnPoint> childs = new List<SpawnPoint>();

    public void Start()
    {
        //LOAD FROM FILE
        char[] values = "1234511111111222222222221111144444444444333333333333355555555555".ToCharArray();
        foreach (char c in values)
        {
            pendingEnemies.Enqueue(spawneableEnemies[c - '1']);
        }

        foreach (Transform child in SP)
        {
            SpawnPoint childHolder = new SpawnPoint();
            childHolder.transformChild = child;
            childHolder.isSpawnable = true;

            childs.Add(childHolder);
        }
    }

    public void Update()
    {
        currentSpawnTime += Time.deltaTime;

        if(currentSpawnTime > spawnTime)
        {
            SpawnEnemy();
            currentSpawnTime -= spawnTime;
        }
    }

    private void SpawnEnemy()
    {
        EnemyStats enemyToSpawn = pendingEnemies.Dequeue();
        Transform spawnPoint = GetLeastCooldownChild().transformChild;
        Enemy e = Instantiate(enemyToSpawn.prefab, spawnPoint.position, Quaternion.identity, null).GetComponent<Enemy>();

        enemyManager.AddSpawnedEnemy(e);
    }

    public SpawnPoint GetRandomChild()
    {
        int randomIndex = Random.Range(0, childs.Count);
        return childs[randomIndex];
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
                Debug.Log("Spawning...");
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
                Debug.Log("Spawning At: " + (randomNum + 1));
                return childs[randomNum + 1];
            }
            else 
            {
                childs[giveChildNum].isSpawnable = true;
                childs[randomNum].isSpawnable = false;
                Debug.Log("Spawning At: " + (randomNum));
                return childs[randomNum];
            }
            
        }


        giveChildNum = Random.Range(0, childs.Count);
        childs[giveChildNum].isSpawnable = false;

        return childs[giveChildNum];
    }
}

