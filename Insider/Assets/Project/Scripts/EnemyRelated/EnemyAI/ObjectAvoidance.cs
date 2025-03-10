using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : EnemyBehaviour
{
    private float rotationSpeed = 5f;
    private float avoidanceStrength = 1.2f;
    private LayerMask mask;
    private Vector2 groupAvoidance = Vector2.zero;

    public override void Behave(Enemy e, Target t)
    {
        Vector2 moveDirection = e.transform.up;
        Vector2 targetDirection = (t.obj.transform.position - e.transform.position).normalized;
        Vector2 individualAvoidance = Vector2.zero;

        mask = LayerMask.GetMask("Terrain");

        Vector2 forward = e.transform.up;
        Vector2 right = Quaternion.Euler(0, 0, -40) * forward;
        Vector2 left = Quaternion.Euler(0, 0, 40) * forward;

        RaycastHit2D hitForward = Physics2D.Raycast(e.transform.position, forward, 2.0f, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(e.transform.position, right, 1.5f, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(e.transform.position, left, 1.5f, mask);

        Debug.DrawRay(e.transform.position, forward * 2.0f, Color.green);
        Debug.DrawRay(e.transform.position, right * 1.5f, Color.red);
        Debug.DrawRay(e.transform.position, left * 1.5f, Color.blue);

        if (hitForward.collider != null)
        {
            individualAvoidance = -forward * avoidanceStrength;
        }
        else if (hitRight.collider != null)
        {
            individualAvoidance = left * avoidanceStrength;
        }
        else if (hitLeft.collider != null)
        {
            individualAvoidance = right * avoidanceStrength;
        }

        Vector2 groupDirection = Vector2.zero;
        int groupSize = 0;
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(e.transform.position, 3.0f);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject != e.gameObject && neighbor.TryGetComponent<Enemy>(out Enemy otherEnemy))
            {
                groupDirection += (Vector2)otherEnemy.transform.up;
                groupSize++;
            }
        }

        if (groupSize > 0)
        {
            groupDirection /= groupSize;
            groupAvoidance = groupDirection;
        }

        Vector2 finalAvoidance = (individualAvoidance * 0.5f) + (groupAvoidance * 0.5f);
        moveDirection = (targetDirection + finalAvoidance).normalized;

        e.transform.up = Vector2.Lerp(e.transform.up, moveDirection, Time.deltaTime * rotationSpeed);
    }
}
