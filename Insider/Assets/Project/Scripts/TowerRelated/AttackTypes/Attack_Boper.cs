using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Boper : MonoBehaviour, IAttackType
{
    public List<GameObject> enemiesOnContact = new List<GameObject>();
    private float lastShotTime = 0f;
    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        Tower tower = GetComponent<Tower>();
        if (Time.time < lastShotTime + 1f / tower.fireRate) return;
        if (enemiesOnContact.Count > 0)
        {
            for (int i = enemiesOnContact.Count - 1; i >= 0; i--)
            {
                enemiesOnContact[i].GetComponent<IDamage>().Damage(GetComponent<Tower>().damage);
            }
        }
        lastShotTime = Time.time;
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
