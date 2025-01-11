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
    }
    public void AddSpawnedEnemy(Enemy e)
    {
        currentEnemy.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        currentEnemy.Remove(e);
        if(currentEnemy.Count <= 0 && spawnManager.currentState.stateName == "finish") 
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
