using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    private Enemy target;
	private Vector3 lastPos;
    bool hasExploded = false;
	private float explosionDuration = 0.5f;
    private float elapsedTime = 0f;
    private Vector3 explosionRadius; //=ProjectileHP
    private Vector3 flyRadius;

    public Tower towerScript;
	public List<GameObject> enemiesOnContact;

    private void Awake()
    {
        flyRadius = transform.localScale;
    }
    void Update()
	{
        Vector2 direction = (lastPos - transform.position).normalized;
		transform.position = Vector2.MoveTowards(transform.position, lastPos, towerScript.projectileSpeed * Time.deltaTime);
		if (transform.position == lastPos)
		{
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / explosionDuration;
            transform.localScale = Vector3.Lerp(flyRadius, explosionRadius, progress);
        }
		if (transform.localScale == explosionRadius) 
		{
            if (enemiesOnContact.Count > 0 && !hasExploded)
            {
                hasExploded = true;
                foreach (GameObject d in enemiesOnContact)
                {
                    d.GetComponent<IDamage>().Damage(towerScript.damage);
                }
                
            }
            Destroy(gameObject);
        }
	}

	public void SetTarget(Enemy newTarget)
    {
		if (target != newTarget) 
		{
            explosionRadius = transform.localScale * towerScript.projectileHp;
            lastPos = newTarget.transform.position;
            target = newTarget;
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
