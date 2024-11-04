using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    protected HashSet<Enemy> currentEnemies = new HashSet<Enemy>();

    public void AddSpawnedEnemy(Enemy e)
    {
        currentEnemies.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        currentEnemies.Remove(e);
    }
}
