using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
	private List<Enemy> chainTargets = new List<Enemy>();
	private int currentTargetIndex = 0;
	public Tower towerScript;

	private float currentSpeed;
	AudioManager audioManager;

	private Enemy CurrentTarget =>
		currentTargetIndex < chainTargets.Count ? chainTargets[currentTargetIndex] : null;

	void Start()
	{
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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

		if (transform.position == CurrentTarget.transform.position) 
		{
                IDamage enemyReference = CurrentTarget.GetComponent<IDamage>();
                if (enemyReference != null)
                {
                    audioManager.PlaySFX_P(12, 0.5f, 1 - (currentTargetIndex / 5));
                    enemyReference.Damage(towerScript.damage);
                }

                if (towerScript.currentLevel == 3)
                {
                    CurrentTarget.Stun(0.5f);
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

	public void SetTargets(List<Enemy> targets)
	{
		chainTargets = targets;
		currentTargetIndex = 0;
	}

}
