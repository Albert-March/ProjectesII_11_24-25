using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingManager : MonoBehaviour
{
    public Enemy GetEnemyTargetFromList(List<Enemy> enemyList, int id)
    {
        switch (id)
        {
            case 0:
                return Firstin(enemyList);
            case 1:
                return LastIn(enemyList);
            case 2:
                return MostHP(enemyList);
            case 3:
                return CloserToEnd(enemyList);
        }
        return null;
    }

    private Enemy Firstin(List<Enemy> enemyList) 
    {
        return enemyList[0];
    }

    private Enemy LastIn(List<Enemy> enemyList)
    {
        return enemyList[enemyList.Count-1];
    }

    private Enemy MostHP(List<Enemy> enemyList)
    {
        Enemy mostHpHolder = enemyList[0];
        foreach(Enemy enemy in enemyList) 
        {
            if (enemy.health > mostHpHolder.health) 
            {
                mostHpHolder = enemy;
            }
        }
        return mostHpHolder;
    }

    private Enemy CloserToEnd(List<Enemy> enemyList)
    {
        Enemy closerToEndHolder = enemyList[0];
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.currentTarget > closerToEndHolder.currentTarget)
            {
                closerToEndHolder = enemy;
            }
        }
        return closerToEndHolder;
    }
}

