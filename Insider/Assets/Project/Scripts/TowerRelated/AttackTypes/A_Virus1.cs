using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Virus1 : MonoBehaviour, IAttackType
{
    public void Attack(Enemy e)
    {
        Vector2 direction = (e.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, e.transform.position);
        RaycastHit2D fire = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(transform.position, e.transform.position - transform.position, Color.blue);
        Debug.Log(fire.transform.name);
        IDamage enemyDmg = fire.transform.GetComponent<IDamage>();
        if (enemyDmg != null)
        {
            enemyDmg.Damage(GetComponent<Tower>().damage);
        }
    }
}