using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : EnemyBehaviour
{
    [SerializeField] private float separationDistance = 1.5f;
    [SerializeField] private float alignmentWeight = 0.7f;
    [SerializeField] private float cohesionWeight = 1.5f;
    [SerializeField] private float separationWeight = 1.2f;
    [SerializeField] private float baseNeighborRadius = 3.0f;
    [SerializeField] private float jitterStrength = 0.2f;
    [SerializeField] private float boidSpeedMultiplier = 1.0f;
    [SerializeField] private float stuckTimeThreshold = 2.0f;
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

        // Find group's midpoint
        if (count > 0)
        {
            groupMidpoint = totalPosition / count;
        }
        else
        {
            groupMidpoint = e.transform.position;
        }

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
            Vector2 unstuckDirection = Quaternion.Euler(0, 0, Random.Range(-90, 90)) * e.transform.up;
            groupAvoidance = unstuckDirection * 2f;
        }
        else
        {
            groupAvoidance = CalculateGroupAvoidance(e);
        }

        Vector2 jitter = new Vector2(Random.Range(-jitterStrength, jitterStrength), Random.Range(-jitterStrength, jitterStrength));

        Vector2 targetDirection = (e.path[e.currentTarget].obj.transform.position - e.transform.position).normalized;
        Vector2 boidDirection = targetDirection + separation * separationWeight + cohesion + alignment + groupAvoidance + jitter;
        boidDirection = boidDirection.normalized * boidSpeedMultiplier;

        e.transform.up = Vector2.Lerp(e.transform.up, boidDirection, Time.deltaTime * 3);
        e.transform.position += (Vector3)(boidDirection * e.movSpeed * Time.deltaTime);
    }

    private Vector2 CalculateGroupAvoidance(Enemy e)
    {
        Vector2 avoidance = Vector2.zero;
        LayerMask mask = LayerMask.GetMask("Terrain");

        Vector2 forward = e.transform.up;
        Vector2 right = Quaternion.Euler(0, 0, -40) * forward;
        Vector2 left = Quaternion.Euler(0, 0, 40) * forward;

        RaycastHit2D hitForward = Physics2D.Raycast(groupMidpoint, forward, 2.5f, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(groupMidpoint, right, 2.0f, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(groupMidpoint, left, 2.0f, mask);

        Debug.DrawRay(groupMidpoint, forward * 2.5f, Color.green);
        Debug.DrawRay(groupMidpoint, right * 2.0f, Color.red);
        Debug.DrawRay(groupMidpoint, left * 2.0f, Color.blue);

        if (hitForward.collider != null)
        {
            avoidance = -forward * 1.5f;
        }
        else if (hitRight.collider != null)
        {
            avoidance = left * 1.5f;
        }
        else if (hitLeft.collider != null)
        {
            avoidance = right * 1.5f;
        }

        return avoidance;
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
