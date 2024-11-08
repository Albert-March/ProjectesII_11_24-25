using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;
	public Tower towerScript;

	void Update()
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}
		Vector2 direction = (target.transform.position - transform.position).normalized;
		transform.position = Vector2.MoveTowards(transform.position, target.transform.position, towerScript.projectileSpeed * Time.deltaTime);
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

	public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == target && !collision.isTrigger)
		{
			IDamage _enemyReference = collision.GetComponent<IDamage>();
			_enemyReference.Damage(towerScript.damage);
            Destroy(gameObject);
		}
	}
}
