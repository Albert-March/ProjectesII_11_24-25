using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyPathfinding : MonoBehaviour
{
    public float detectionRange = 100f;
    public float avoidForce = 100f;
    public float seekForce = 300f;

    public void MoveTowardsTarget(Enemy enemy, Target t)
    {
        Vector2 desiredVelocity = ((Vector2)(t.obj.transform.position - enemy.transform.position)).normalized * seekForce;
        Vector2 steering = desiredVelocity;

        // Check for obstacles using raycasts
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, (Vector2)enemy.transform.up, detectionRange);

        if (hit.collider.tag == "Wall")
        {
            // If an obstacle is detected, calculate an avoidance force
            Vector2 avoidanceForce = Vector2.Perpendicular(hit.normal).normalized * avoidForce;
            steering += avoidanceForce;  // Add the avoidance force to the seek force
        }

        // Apply the steering force to move the enemy
        Vector2 newVelocity = steering.normalized * enemy.movSpeed;
        enemy.transform.position += (Vector3)(newVelocity * Time.deltaTime);

        // Optional: Rotate to face the movement direction
        if (newVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}