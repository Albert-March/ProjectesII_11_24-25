using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Detection : MonoBehaviour
{
    public Transform target; // The target the AI should follow
    public float obstacleDetectionRadius = 5.0f; // Radius for obstacle avoidance
    public int contextMapResolution = 36; // Number of slots in the context map

    private float[] interestMap;
    private float[] dangerMap;
    private Vector2[] directions;

    void Start()
    {

        obstacleDetectionRadius = GetComponent<CircleCollider2D>().radius;

        interestMap = new float[contextMapResolution];
        dangerMap = new float[contextMapResolution];
        directions = new Vector2[contextMapResolution];

        // Precompute directions for each slot in the context map
        float angleStep = 360.0f / contextMapResolution;
        for (int i = 0; i < contextMapResolution; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    public void ClearContextMaps()
    {
        // Reset maps each frame
        for (int i = 0; i < contextMapResolution; i++)
        {
            interestMap[i] = 0f;
            dangerMap[i] = 0f;
        }
    }

    protected void UpdateInterestMap(Target t)
    {
        // Compute direction toward the target and add it to interest map
        Vector2 toTarget = t.obj.transform.position - transform.position;
        int targetSlot = GetClosestDirectionSlot(toTarget.normalized);
        interestMap[targetSlot] = 1.0f / toTarget.magnitude; // Higher interest for closer targets
    }

    protected void UpdateDangerMap()
    {
        // Detect obstacles within the specified radius and filter for objects tagged as "wall"
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, obstacleDetectionRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            // Ensure the detected object is not the AI itself and is tagged as "wall"
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                Vector2 toObstacle = hit.point - (Vector2)transform.position;
                int obstacleSlot = GetClosestDirectionSlot(toObstacle.normalized);
                dangerMap[obstacleSlot] = Mathf.Max(dangerMap[obstacleSlot], 1.0f / toObstacle.magnitude); // Higher danger for closer obstacles
            }
        }
    }

    protected int GetClosestDirectionSlot(Vector2 direction)
    {
        int bestSlot = 0;
        float maxDot = Vector2.Dot(directions[0], direction);
        for (int i = 1; i < directions.Length; i++)
        {
            float dot = Vector2.Dot(directions[i], direction);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestSlot = i;
            }
        }
        return bestSlot;
    }

    protected void MoveInBestDirection(Enemy e)
    {
        float[] combinedMap = new float[contextMapResolution];
        bool avoidMode = false;

        // Check if danger level in any direction is above threshold
        for (int i = 0; i < contextMapResolution; i++)
        {
            if (dangerMap[i] > 0.7f) // Threshold to trigger avoidance mode
            {
                avoidMode = true;
                break;
            }
        }

        if (avoidMode)
        {
            // Avoidance mode: ignore interest and choose the safest direction
            int safestDirectionIndex = 0;
            float lowestDanger = dangerMap[0];

            for (int i = 1; i < dangerMap.Length; i++)
            {
                if (dangerMap[i] < lowestDanger)
                {
                    lowestDanger = dangerMap[i];
                    safestDirectionIndex = i;
                }
            }

            // Move in the safest direction
            Vector2 safestDirection = directions[safestDirectionIndex];
            transform.position += (Vector3)(safestDirection * e.movSpeed * Time.deltaTime * 0.5f); // Move slowly while avoiding
        }
        else
        {
            // Normal mode: combine interest and danger maps
            for (int i = 0; i < contextMapResolution; i++)
            {
                combinedMap[i] = interestMap[i] - (dangerMap[i] * 2.0f); // Amplify danger influence
            }

            int bestDirectionIndex = 0;
            float bestValue = combinedMap[0];

            for (int i = 1; i < combinedMap.Length; i++)
            {
                if (combinedMap[i] > bestValue)
                {
                    bestValue = combinedMap[i];
                    bestDirectionIndex = i;
                }
            }

            // Move in the best combined direction
            Vector2 bestDirection = directions[bestDirectionIndex];
            transform.position += (Vector3)(bestDirection * e.movSpeed * Time.deltaTime);
        }
    }
}
