using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleDMG : MonoBehaviour
{
    public float damagePerSecond = 5f;
    public float tickInterval = 1f;
    public float duration = 10f;

    private float lifeTimer = 0f;
    private float tickTimer = 0f;

    private Vector3 initialScale;


    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(lifeTimer / duration);
        transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, progress);

        if (lifeTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        if (tickTimer >= tickInterval)
        {
            ApplyTickDamage();
            tickTimer = 0f;
        }
    }

    void ApplyTickDamage()
    {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemiesInRange[i];
            if (enemy != null)
                enemy.GetComponent<IDamage>().Damage(damagePerSecond);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger) return;

        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
                enemiesInRange.Remove(enemy);
        }
    }
}

