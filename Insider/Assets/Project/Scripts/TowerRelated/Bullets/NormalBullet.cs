using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public Enemy target;
    public Tower towerScript;
    AudioManager audioManager;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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

            if (transform.position == target.transform.position) 
            {
                audioManager.PlaySFX(12, 0.5f);
                IDamage _enemyReference = target.GetComponent<IDamage>();
                _enemyReference.Damage(towerScript.damage);
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Enemy newTarget)
    {
        target = newTarget;
    }
}
