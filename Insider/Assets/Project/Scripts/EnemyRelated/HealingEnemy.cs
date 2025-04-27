using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingEnemy : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;

	[SerializeField] private ParticleSystem healingParticles;
	private ParticleSystem healingParticlesInstance;

	public float healingAmount = 5f;
	public float healingInterval = 0.5f;

	private float timer = 0f;
	public List<GameObject> enemiesInRange = new List<GameObject>();

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= healingInterval)
		{
			HealEnemies();
			timer = 0f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			enemiesInRange.Add(collision.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			enemiesInRange.Remove(collision.gameObject);
		}
	}

	private void HealEnemies()
	{
		foreach (GameObject enemy in enemiesInRange)
		{
			if (enemy != null && enemy.TryGetComponent<IHealable>(out IHealable healable))
			{
				healable.Heal(healingAmount);
				SpawnParticles(enemy.transform.position);
			}
		}
	}

	public void SpawnParticles(Vector3 position)
	{
		if (healingParticles != null)
		{
			Instantiate(healingParticles, position, Quaternion.identity);
		}
	}


}
