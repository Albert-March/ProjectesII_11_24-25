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
        Debug.Log(EnemiesOnScreen);
        if (spawnManager.currentState.stateName == "finish" && !spawnManager.isInDelayState)
        {
            SceneManager.LoadScene("WinScreen");
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
