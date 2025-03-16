using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageEnemy : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;

	[SerializeField] private ParticleSystem rageParticles;
	private ParticleSystem rageParticlesInstance;
	public float speedBoostAmount = 5f;

	public List<GameObject> enemiesInRange = new List<GameObject>();

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Destroy(gameObject, 5f);
	}

	private void Update()
	{
		RageEnemies();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy") && collision.TryGetComponent<IMovable>(out IMovable movable))
		{
			enemiesInRange.Add(collision.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy") && collision.TryGetComponent<IMovable>(out IMovable movable))
		{
			enemiesInRange.Remove(collision.gameObject);
			movable.ResetSpeed();
		}
	}

	private void RageEnemies()
	{
		foreach (GameObject enemy in enemiesInRange)
		{
			if (enemy != null && enemy.TryGetComponent<IMovable>(out IMovable movable))
			{
				movable.Rage(speedBoostAmount);
			}
		}
	}

	public void SpawnParticles(Vector3 position)
	{
		if (rageParticles != null)
		{
			Instantiate(rageParticles, position, Quaternion.identity);
		}
	}
}
