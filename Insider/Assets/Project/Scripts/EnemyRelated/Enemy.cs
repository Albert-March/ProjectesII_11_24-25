using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyPathfinding pf;

    public string enemyName;
    public float movSpeed;
    public float health;
    public float dmg;
    public int economyGiven;
    SpriteRenderer sprite;
    public List<Target> path;
    private int currentTarget;

    public void SetEnemyData(EnemyStats enemy)
    {
        this.enemyName = enemy.enemyName;
        this.movSpeed = enemy.movSpeed;
        this.health = enemy.health;
        this.dmg = enemy.dmg;
        this.economyGiven = enemy.economyGiven;
        this.sprite.color = enemy.color;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if(currentTarget < path.Count)
        {
            pf.MoveTowardsTarget(this, path[currentTarget]);
        }
        else
        {
            pf.MoveTowardsTarget(this, path[0]);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(currentTarget < path.Count){
            if(col.transform == path[currentTarget].obj.transform)
            {
                Debug.Log(currentTarget);
                currentTarget++;
            }
        }
        else
        {
            //El Target sera el parasit
        }
    }
}
