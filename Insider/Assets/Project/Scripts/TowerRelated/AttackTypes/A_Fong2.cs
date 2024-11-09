using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Fong2 : MonoBehaviour, IAttackType
{
    public List<GameObject> enemiesOnContact = new List<GameObject>();
    public void Attack(Enemy e)
    {
        if (enemiesOnContact.Count > 0)
        {
            foreach (GameObject d in enemiesOnContact)
            {
                d.GetComponent<IDamage>().Damage(GetComponent<Tower>().damage);
            }
        }
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
