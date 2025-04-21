using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBullet : MonoBehaviour
{
	private List<Enemy> chainTargets = new List<Enemy>();
	private int currentTargetIndex = 0;
	public Tower towerScript;

	public GameObject impactEffectPrefab;
	public GameObject chainEffectPrefab;

	private float currentSpeed;

	private Enemy CurrentTarget =>
		currentTargetIndex < chainTargets.Count ? chainTargets[currentTargetIndex] : null;

	void Start()
	{
		currentSpeed = towerScript.projectileSpeed; // velocidad inicial normal
	}

	void Update()
	{
		if (CurrentTarget == null || CurrentTarget.gameObject == null)
		{
			Destroy(gameObject);
			return;
		}

		// Movimiento hacia el objetivo actual
		Vector2 direction = (CurrentTarget.transform.position - transform.position).normalized;
		transform.position = Vector2.MoveTowards(transform.position, CurrentTarget.transform.position, currentSpeed * Time.deltaTime);

		// Rotación hacia el objetivo
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
			// Hacer daño
			IDamage enemyReference = collision.GetComponent<IDamage>();
			if (enemyReference != null)
			{
				enemyReference.Damage(towerScript.damage);
			}

			// Efecto visual de impacto
			if (impactEffectPrefab != null)
			{
				Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
			}

			currentTargetIndex++;

			// Validar próximos objetivos
			while (currentTargetIndex < chainTargets.Count &&
				   (chainTargets[currentTargetIndex] == null || chainTargets[currentTargetIndex].gameObject == null))
			{
				currentTargetIndex++;
			}

			// Si ya no hay más objetivos, destruir
			if (currentTargetIndex >= chainTargets.Count)
			{
				Destroy(gameObject);
				return;
			}

			// Efecto de rayo entre rebotes
			if (chainEffectPrefab != null)
			{
				Vector3 from = transform.position;
				Vector3 to = chainTargets[currentTargetIndex].transform.position;
				GameObject effect = Instantiate(chainEffectPrefab, from, Quaternion.identity);
				LineRenderer lr = effect.GetComponent<LineRenderer>();
				if (lr != null)
				{
					lr.SetPosition(0, from);
					lr.SetPosition(1, to);
				}
				Destroy(effect, 0.3f);
			}

			// Reducir velocidad después de cada rebote
			currentSpeed *= 0.8f; // Reduce un 20% por rebote
		}
	}
}
