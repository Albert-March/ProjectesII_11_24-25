using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagBullet : MonoBehaviour
{
    private Enemy target;
    private Vector3 lastPos;

    public int tagHp;
    public A_Fong1 fatherBullets;
    private float dmgCooldown = 0;
    private float timeToDeleate = 0;

    public Tower towerScript;
    public List<GameObject> enemiesOnContact;

    private void Start()
    {
        timeToDeleate = Time.time;
    }
    void Update()
    {
        if (Time.time >= dmgCooldown + 1) //Els tics De da�o son en base al firerate de la torre
        {
            if (enemiesOnContact.Count > 0)
            {
                foreach (GameObject d in enemiesOnContact)
                {
                    d.GetComponent<IDamage>().Damage(towerScript.damage);
                }
                tagHp--;
                dmgCooldown = Time.time;
            }
        }


        if (tagHp <= 0 || (Time.time >= timeToDeleate + 30 && enemiesOnContact.Count < 1)) 
        {
            fatherBullets.amountOfTagsCreated.Remove(gameObject);
            Destroy(gameObject);
        } 

    }

    public void SetTarget(Vector2 objectpos, A_Fong1 father)
    {
        fatherBullets = father;
        transform.position = objectpos;
        tagHp = towerScript.projectileHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && !collision.isTrigger)
        {
            GameObject _enemyReference = collision.gameObject;
            enemiesOnContact.Add(_enemyReference);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && !collision.isTrigger)
        {
            GameObject _enemyReference = collision.gameObject;
            enemiesOnContact.Remove(_enemyReference);
        }
    }
}
