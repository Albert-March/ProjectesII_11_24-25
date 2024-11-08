using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    protected HashSet<Enemy> currentTower = new HashSet<Enemy>();

    public void AddSpawnedEnemy(Enemy e)
    {
        currentTower.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
		currentTower.Remove(e);
    }
}
