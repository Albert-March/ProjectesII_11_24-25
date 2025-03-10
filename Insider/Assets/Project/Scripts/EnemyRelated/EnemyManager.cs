using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    protected HashSet<Enemy> currentEnemy = new HashSet<Enemy>();

    public SpawnManager spawnManager;

    public int EnemiesOnScreen;

    public void Update()
    {
        EnemiesOnScreen = currentEnemy.Count;
        if (spawnManager.currentState.stateName == "finish" && !spawnManager.isInDelayState)
        {
            S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
            transition.CallPass("WinScreen");
        }
    }
    public void AddSpawnedEnemy(Enemy e)
    {
        currentEnemy.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        currentEnemy.Remove(e);
    }
}
