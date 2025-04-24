using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
	private List<Enemy> chainTargets = new List<Enemy>();
	private int currentTargetIndex = 0;
	public Tower towerScript;

	private float currentSpeed;

	private Enemy CurrentTarget =>
		currentTargetIndex < chainTargets.Count ? chainTargets[currentTargetIndex] : null;

	void Start()
	{
		currentSpeed = towerScript.projectileSpeed;
	}

	void Update()
	{
		if (CurrentTarget == null || CurrentTarget.gameObject == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector2 direction = (CurrentTarget.transform.position - transform.position).normalized;
		transform.position = Vector2.MoveTowards(transform.position, CurrentTarget.transform.position, currentSpeed * Time.deltaTime);

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void SetTargets(List<Enemy> targets)
	{
		chainTargets = targets;
		currentTargetIndex = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (CurrentTarget == null || collision.isTrigger) return;

		if (collision.gameObject == CurrentTarget.gameObject)
		{
			IDamage enemyReference = collision.GetComponent<IDamage>();
			if (enemyReference != null)
			{
				enemyReference.Damage(towerScript.damage);
			}

            if (towerScript.currentLevel == 3)
            {
                StartCoroutine(ApplyStun(CurrentTarget, 0.5f));
            }

            currentTargetIndex++;

			while (currentTargetIndex < chainTargets.Count &&
				   (chainTargets[currentTargetIndex] == null || chainTargets[currentTargetIndex].gameObject == null))
			{
				currentTargetIndex++;
			}

			if (currentTargetIndex >= chainTargets.Count)
			{
				Destroy(gameObject);
				return;
			}

			currentSpeed *= 0.8f;
		}
	}
    private IEnumerator ApplyStun(Enemy enemy, float duration)
    {
        if (enemy == null) yield break;

        enemy.isStuned = true;

        yield return new WaitForSeconds(duration);

        if (enemy != null)
            enemy.isStuned = false;
    }
}
