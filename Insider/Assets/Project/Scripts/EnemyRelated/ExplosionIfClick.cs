using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionIfClick : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public float damage = 50f;

	private float timer = 0f;
	private bool explode = false;

	private List<GameObject> enemiesInRange = new List<GameObject>();
	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	public void Explode()
	{
		explode = true;
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer >= 5f)
		{
			Destroy(gameObject);
		}

		if (explode)
		{
			timer += Time.deltaTime;

			if (timer <= 5f)
			{
				transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0f);
				spriteRenderer.color = Color.red;
			}
			else
			{
				DealDamageToEnemies();
				Destroy(gameObject);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Agrega enemigos dentro del rango al entrar
		if (collision.CompareTag("Enemy"))
		{
			enemiesInRange.Add(collision.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// Elimina enemigos al salir del rango
		if (collision.CompareTag("Enemy"))
		{
			enemiesInRange.Remove(collision.gameObject);
		}
	}

	private void DealDamageToEnemies()
	{
		foreach (GameObject enemy in enemiesInRange)
		{
			if (enemy != null && enemy.TryGetComponent<IDamage>(out IDamage damageable))
			{
				damageable.Damage(damage);
			}
		}
	}
}
