using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Boper : MonoBehaviour, IAttackType
{
    public List<GameObject> enemiesOnContact = new List<GameObject>();
    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        if (enemiesOnContact.Count > 0)
        {
            for (int i = enemiesOnContact.Count - 1; i >= 0; i--)
            {
                enemiesOnContact[i].GetComponent<IDamage>().Damage(GetComponent<Tower>().damage);
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
