using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private Enemy target;
    public Tower towerScript;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            towerScript.lastShootTime += towerScript.fireRate;
            return;
        }
        else 
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, towerScript.projectileSpeed * Time.deltaTime);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void SetTarget(Enemy newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null) 
        {
            if (collision.gameObject == target.gameObject && !collision.isTrigger)
            {
                IDamage _enemyReference = collision.GetComponent<IDamage>();
                _enemyReference.Damage(towerScript.damage);
                Destroy(gameObject);
            }
        }
    }
}
