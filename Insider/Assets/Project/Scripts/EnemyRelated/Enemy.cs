using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float movSpeed;
    public float health;
    public float dmg;
    public int economyGiven;
    SpriteRenderer sprite;
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
        sprite.color = Color.red;
    }
}
