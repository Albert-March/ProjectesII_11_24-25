using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_Virus1 : AttackManager
{
    private float lastShootTime;
    private Enemy targetEnemy;
    private Tower towerScript;

    private void Awake()
    {
        towerScript = GetComponent<Tower>();
        lastShootTime = -towerScript.fireRate;
    }

    public override void Attack(Enemy e)
    {
        targetEnemy = e;
        if (Time.time >= lastShootTime + towerScript.fireRate)
        {
            Railgun();
            lastShootTime = Time.time;
        }
    }

    private void Railgun()
    {
        RaycastHit2D fire = Physics2D.Raycast(towerScript.transform.position, targetEnemy.transform.position);
        Debug.DrawRay(towerScript.transform.position, targetEnemy.transform.position, Color.green);
    }
}



//public GameObject bulletPrefab;
//public Transform firePoint;

//void ShootAtTarget(GameObject target)
//{
//    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
//    // Asignar objetiu
//    Bullet bulletScript = bullet.GetComponent<Bullet>();
//    bulletScript.towerScript = towerScript;
//    if (bulletScript != null)
//    {
//        bulletScript.SetTarget(target);
//    }
//}