using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : EnemyBehaviour
{
    [SerializeField] private float separationDistance = 1.2f;
    [SerializeField] private float alignmentWeight = 0.6f;
    [SerializeField] private float cohesionWeight = 1.2f;
    [SerializeField] private float separationWeight = 0.8f;
    [SerializeField] private float baseNeighborRadius = 2.5f;
    [SerializeField] private float jitterStrength = 0.1f;
    [SerializeField] private float boidSpeedMultiplier = 1.0f;
    [SerializeField] private float stuckTimeThreshold = 1.5f;
    [SerializeField] private float avoidanceStrength = 1.8f; // Reduced to prevent sharp turns
    [SerializeField] private float wallDetectionDistance = 2.8f;
    private Vector2 groupMidpoint = Vector2.zero;
    private Vector2 groupAvoidance = Vector2.zero;
    private float timeStuck = 0.0f;

    public override void Behave(Enemy e, Target t)
    {
        List<Enemy> closeEnemies = new List<Enemy>();
        Vector2 totalPosition = Vector2.zero;
        int count = 0;

        Collider2D[] allEnemies = Physics2D.OverlapCircleAll(e.transform.position, baseNeighborRadius);
        foreach (var col in allEnemies)
        {
            if (col.gameObject != e.gameObject && col.TryGetComponent<Enemy>(out Enemy otherEnemy))
            {
                if (otherEnemy.currentTarget == e.currentTarget)
                {
                    closeEnemies.Add(otherEnemy);
                    totalPosition += (Vector2)otherEnemy.transform.position;
                    count++;
                }
            }
        }

        groupMidpoint = (count > 0) ? totalPosition / count : e.transform.position;
        float adjustedNeighborRadius = Mathf.Max(1.5f, baseNeighborRadius - (count * 0.2f));

        Vector2 separation = Vector2.zero;
        Vector2 alignment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        bool shouldSwitchTarget = false;
        int neighborCount = 0;

        foreach (Enemy otherEnemy in closeEnemies)
        {
            if (Vector2.Distance(otherEnemy.transform.position, groupMidpoint) < adjustedNeighborRadius)
            {
                if (otherEnemy.currentTarget > e.currentTarget)
                {
                    shouldSwitchTarget = true;
                }

                Vector2 diff = (Vector2)(e.transform.position - otherEnemy.transform.position);
                if (diff.magnitude < separationDistance)
                {
                    separation += diff.normalized / diff.magnitude;
                }

                alignment += (Vector2)otherEnemy.transform.up;
                cohesion += (Vector2)otherEnemy.transform.position;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            alignment /= neighborCount;
            alignment = alignment.normalized * alignmentWeight;

            cohesion /= neighborCount;
            cohesion = ((cohesion - (Vector2)e.transform.position).normalized) * cohesionWeight;
        }

        if (shouldSwitchTarget && e.currentTarget < e.path.Count - 1)
        {
            e.currentTarget++;
        }

        if (DetectStuck(e))
        {
            groupAvoidance = SteerOutOfObstacle(e);
        }
        else
        {
            groupAvoidance = CalculateGroupAvoidance(e);
        }

        Vector2 jitter = new Vector2(Random.Range(-jitterStrength, jitterStrength), Random.Range(-jitterStrength, jitterStrength));

        // 🔹 **Final movement direction with smoother blending**
        Vector2 targetDirection = (e.path[e.currentTarget].obj.transform.position - e.transform.position).normalized;
        Vector2 boidDirection = Vector2.Lerp(targetDirection,
            separation * separationWeight + cohesion * 0.9f + alignment + groupAvoidance * avoidanceStrength + jitter, 0.3f);

        boidDirection = boidDirection.normalized * boidSpeedMultiplier;

        // **Smooth Rotation using `Slerp` (Prevents Sharp Turns)**
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, boidDirection);
        e.transform.rotation = Quaternion.Slerp(e.transform.rotation, targetRotation, Time.deltaTime * 4); // Adjust turning speed

        e.transform.position += (Vector3)(boidDirection * e.movSpeed * Time.deltaTime);
    }

    // **🚀 Improved Wall Avoidance (Less Overpowering) 🚀**
    private Vector2 CalculateGroupAvoidance(Enemy e)
    {
        Vector2 avoidance = Vector2.zero;
        LayerMask mask = LayerMask.GetMask("Terrain");

        Vector2 forward = e.transform.up;
        Vector2 right45 = Quaternion.Euler(0, 0, -45) * forward;
        Vector2 left45 = Quaternion.Euler(0, 0, 45) * forward;

        RaycastHit2D hitForward = Physics2D.Raycast(groupMidpoint, forward, wallDetectionDistance, mask);
        RaycastHit2D hitRight45 = Physics2D.Raycast(groupMidpoint, right45, wallDetectionDistance - 1.0f, mask);
        RaycastHit2D hitLeft45 = Physics2D.Raycast(groupMidpoint, left45, wallDetectionDistance - 1.0f, mask);

        if (hitForward.collider != null)
        {
            avoidance += -forward * 2.5f;
        }
        if (hitRight45.collider != null)
        {
            avoidance += left45 * 1.8f;
        }
        if (hitLeft45.collider != null)
        {
            avoidance += right45 * 1.8f;
        }

        return avoidance.normalized;
    }

    // **🚀 Improved Stuck Fix (Enemies Stay in Motion) 🚀**
    private Vector2 SteerOutOfObstacle(Enemy e)
    {
        return Quaternion.Euler(0, 0, Random.Range(-45, 45)) * e.transform.up * 1.5f;
    }

    private bool DetectStuck(Enemy e)
    {
        if (Physics2D.Raycast(e.transform.position, e.transform.up, 1.5f, LayerMask.GetMask("Terrain")))
        {
            timeStuck += Time.deltaTime;
            if (timeStuck > stuckTimeThreshold)
            {
                timeStuck = 0;
                return true;
            }
        }
        else
        {
            timeStuck = 0;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groupMidpoint, baseNeighborRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groupMidpoint, 1.0f);
    }
}
