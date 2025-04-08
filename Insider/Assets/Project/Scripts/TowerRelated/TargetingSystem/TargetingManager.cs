using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingManager : MonoBehaviour
{
    public List<Enemy> GetEnemyTargetFromList(List<Enemy> enemyList, int amount, int id)
    {
        switch (id)
        {
            case 0:
                return Firstin(enemyList, amount);
            case 1:
                return LastIn(enemyList, amount);
            case 2:
                return MostHP(enemyList, amount);
            case 3:
                return LeastHP(enemyList, amount);
            case 4:
                return CloserToEnd(enemyList, amount);


        }
        return null;
    }

    private List<Enemy> Firstin(List<Enemy> enemyList, int amount)
    {
        if (amount == 0) 
        {
            return enemyList;
        }
        List<Enemy> enemiesToSend = new List<Enemy>();

        int count = Mathf.Min(amount, enemyList.Count);
        for (int i = 0; i < count; i++)
        {
            enemiesToSend.Add(enemyList[i]);
        }

        return enemiesToSend;
    }

    private List<Enemy> LastIn(List<Enemy> enemyList, int amount)
    {
        if (amount == 0)
        {
            return enemyList;
        }
        List<Enemy> enemiesToSend = new List<Enemy>();

        int count = Mathf.Min(amount, enemyList.Count);
        for (int i = 0; i < count; i++)
        {
            int index = enemyList.Count - 1 - i; // correct offset from the end
            enemiesToSend.Add(enemyList[index]);
        }

        return enemiesToSend;
    }

    private List<Enemy> MostHP(List<Enemy> enemyList, int amount)
    {
        if (amount == 0)
        {
            return enemyList;
        }
        List<Enemy> enemiesToSend = new List<Enemy>();

        for (int i = 0; i < amount; i++)
        {
            Enemy highest = null;
            float highestHP = float.MinValue;

            foreach (Enemy enemy in enemyList)
            {
                if (enemy == null || enemiesToSend.Contains(enemy)) continue;

                if (enemy.health > highestHP)
                {
                    highestHP = enemy.health;
                    highest = enemy;
                }
            }

            if (highest != null)
            {
                enemiesToSend.Add(highest);
            }
            else
            {
                break;
            }
        }

        return enemiesToSend;
    }

    private List<Enemy> LeastHP(List<Enemy> enemyList, int amount)
    {
        if (amount == 0)
        {
            return enemyList;
        }
        List<Enemy> enemiesToSend = new List<Enemy>();

        for (int i = 0; i < amount; i++)
        {
            Enemy lowest = null;
            float lowestHP = float.MaxValue;

            foreach (Enemy enemy in enemyList)
            {
                if (enemy == null || enemiesToSend.Contains(enemy)) continue;

                if (enemy.health < lowestHP)
                {
                    lowestHP = enemy.health;
                    lowest = enemy;
                }
            }

            if (lowest != null)
            {
                enemiesToSend.Add(lowest);
            }
            else
            {
                break;
            }
        }

        return enemiesToSend;
    }

    private List<Enemy> CloserToEnd(List<Enemy> enemyList, int amount)
    {
        if (amount == 0)
        {
            return enemyList;
        }
        List<Enemy> enemiesToSend = new List<Enemy>();

        for (int i = 0; i < amount; i++)
        {
            Enemy closest = null;
            float maxProgress = float.MinValue;

            foreach (Enemy enemy in enemyList)
            {
                if (enemy == null || enemiesToSend.Contains(enemy)) continue;

                if (enemy.currentTarget > maxProgress)
                {
                    maxProgress = enemy.currentTarget;
                    closest = enemy;
                }
            }

            if (closest != null)
            {
                enemiesToSend.Add(closest);
            }
            else
            {
                break;
            }
        }

        return enemiesToSend;
    }
}

