using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TongeBullet : MonoBehaviour
{
    public GameObject sprite;
    public Enemy target;

    private Vector2 direction;
    private float angle;
    private float initialDistance;
    private float currentDistance = 0f;
    private float extractSpeed;
    private float retractSpeed;

    private IDamage enemyDmg;
    private bool isRetracting = false;

    void Start()
    {
        extractSpeed = 20f;
        retractSpeed = extractSpeed / 5;

        if (target == null)
        {
            Debug.LogWarning("No target assigned for TongeBullet.");
            Destroy(gameObject);
            return;
        }

        // Calculate direction and distances
        direction = (target.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        initialDistance = Vector2.Distance(transform.position, target.transform.position);

        enemyDmg = target.GetComponent<IDamage>();

        // Initialize sprite size
        sprite.GetComponent<SpriteRenderer>().size = new Vector2(0, sprite.GetComponent<SpriteRenderer>().size.y);
    }

    void Update()
    {
        if (!isRetracting)
        {
            // Rotate sprite
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Expand tongue
            currentDistance += extractSpeed * Time.deltaTime;
            if (currentDistance >= initialDistance)
            {
                currentDistance = initialDistance;

                // Damage the enemy when reaching maximum extension
                if (enemyDmg != null && target != null)
                {
                    enemyDmg.Damage(target.health);
                }

                // Start retracting
                isRetracting = true;
            }
        }
        else
        {
            // Retract tongue
            currentDistance -= retractSpeed * Time.deltaTime;
            if (currentDistance <= 0)
            {
                currentDistance = 0;

                // Destroy the bullet once fully retracted
                Destroy(gameObject);
                return;
            }
        }

        // Update the sprite size and position
        UpdateSpriteSize(currentDistance);
    }

    private void UpdateSpriteSize(float currentDistance)
    {
        // Update the sprite size and position
        sprite.GetComponent<SpriteRenderer>().size = new Vector2(currentDistance, sprite.GetComponent<SpriteRenderer>().size.y);
        sprite.transform.position = (Vector2)transform.position + direction * (currentDistance / 2);
    }

}
