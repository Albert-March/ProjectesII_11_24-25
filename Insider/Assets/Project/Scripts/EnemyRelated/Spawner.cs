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

    private float spawnTime = 1f;
    private float currentSpawnTime = 0.0f;

    public EnemyManager enemyManager;
    public TargetManager targetManager;

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

        //LOAD FROM FILE
        char[] values = "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111121211112112111212111111211121211112212111212211211122222212222212221222222122222222222223222322322323223232323333333333333333333333333344333344334333444334434434444434444444454444445555".ToCharArray();
        foreach (char c in values)
        {
            pendingEnemies.Enqueue(spawneableEnemies[c - '1']);
        }
        Debug.Log(pendingEnemies.Count);
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
        if (pendingEnemies.Count != 0) 
        {
            EnemyStats enemyToSpawn = pendingEnemies.Dequeue();
            Transform spawnPoint = GetLeastCooldownChild().transformChild;
            Enemy e = Instantiate(enemyToSpawn.prefab, spawnPoint.position, Quaternion.identity, null).GetComponent<Enemy>();
            e.SetEnemyData(enemyToSpawn);
            e.enemyManager = enemyManager;
            e.path = targetManager.GetRandomPath();


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
}

