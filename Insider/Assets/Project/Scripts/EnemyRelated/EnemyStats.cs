using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
    {
        //public int positionInArray;
        public string enemyName;
        public float movSpeed;
        public float health;
        public float dmg;
        public int economyGiven;
        public Color color;
        public GameObject prefab;
    }

/*
    
    class Splitter : EnemyStats
    {
        public override void SetEnemy()
        {
            enemyName = "IgG";
            movSpeed = 1.0f;
            health = 150.0f;
            dmg = 10.0f;
            economyGiven = 10;
            color = Color.yellow;
        }

        public void Split()
        {

        }
    }
    
    class Tank : EnemyStats
    {
        public override void SetEnemy()
        {
            enemyName = "IgM";
            movSpeed = 0.5f;
            health = 500.0f;
            dmg = 10.0f;
            economyGiven = 60;
            color = Color.black;
        }
    }
*/