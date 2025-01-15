using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionIfClick : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	[SerializeField] public Sprite newSprite;

	[SerializeField] private ParticleSystem destroyParticles;
	private ParticleSystem destroyParticlesInstance;
    public GameObject button;
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
        button.GetComponent<Image>().raycastTarget = false;
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
				spriteRenderer.sprite = newSprite;
			}
			else
			{
				SpawnParticles();
				DealDamageToEnemies();
			}
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

	private void DealDamageToEnemies()
	{
		foreach (GameObject enemy in enemiesInRange)
		{
			if (enemy != null && enemy.TryGetComponent<IDamage>(out IDamage damageable))
			{
				damageable.Damage(damage);
			}
		}
        Destroy(gameObject);
    }

	public void SpawnParticles()
	{
		destroyParticlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
	}
}
