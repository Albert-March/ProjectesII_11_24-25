using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidance : EnemyBehaviour
{
    private float rotationSpeed;

    private Vector2 currentAngle;
    private Vector2 nextAngle;

    private LayerMask mask;
    private Vector2 RotateOvertime(Vector2 nA)
    {
        Vector2 cA = transform.up;
        currentAngle = cA;
        if (cA != nA)
        {
            return Vector2.Lerp(cA, nA, rotationSpeed * Time.deltaTime);
        }
        else
        {
            return cA;
        }
    }

    public override void Behave(Enemy e, Target t) 
    {
        e.transform.up = RotateOvertime(nextAngle);

        Vector2 frontAngle1 = Quaternion.Euler(0, 0, -30) * transform.up;
        Vector2 frontAngle2 = Quaternion.Euler(0, 0, 30) * transform.up;
        Vector2 rightAngle = Quaternion.Euler(0, 0, -35) * transform.up;
        Vector2 leftAngle = Quaternion.Euler(0, 0, 35) * transform.up;

        mask = LayerMask.GetMask("Terrain");

        RaycastHit2D front1 = Physics2D.Raycast(e.transform.position, frontAngle1, frontAngle1.magnitude * 2.5f, mask);
        RaycastHit2D front2 = Physics2D.Raycast(e.transform.position, frontAngle2, frontAngle2.magnitude * 2.5f, mask);
        RaycastHit2D right = Physics2D.Raycast(e.transform.position, rightAngle, rightAngle.magnitude * 2, mask);
        RaycastHit2D left = Physics2D.Raycast(e.transform.position, leftAngle, rightAngle.magnitude * 2, mask);

        //Debug.DrawRay(e.transform.position, frontAngle1 * 2.5f, Color.green);
        //Debug.DrawRay(e.transform.position, frontAngle2 * 2.5f, Color.green);

        //Debug.DrawRay(e.transform.position, rightAngle * 2, Color.red);
        //Debug.DrawRay(e.transform.position, leftAngle * 2, Color.blue);


        if (right.collider != null && left.collider != null)
        {
            if (right.distance < left.distance)
            {
                nextAngle = Quaternion.Euler(0, 0, rotationSpeed) * transform.up;
            }
            else if (right.distance > left.distance)
            {
                nextAngle = Quaternion.Euler(0, 0, -rotationSpeed) * transform.up;
            }
            else
            {
                nextAngle = Quaternion.Euler(0, 0, -rotationSpeed) * transform.up;
            }
        }
        else if (right.collider != null && (front1.collider != null || front2.collider != null))
        {
            rotationSpeed = (((frontAngle1.magnitude * 2.5f) + (frontAngle2.magnitude * 2.5f)) - (front1.distance + front2.distance)) * 3;
            nextAngle = Quaternion.Euler(0, 0, rotationSpeed) * transform.up;
        }
        else if (left.collider != null && (front1.collider != null || front2.collider != null))
        {
            rotationSpeed = (((frontAngle1.magnitude * 2.5f) + (frontAngle2.magnitude * 2.5f)) - (front1.distance + front2.distance)) * 3;
            nextAngle = Quaternion.Euler(0, 0, -rotationSpeed) * transform.up;
        }
        else
        {
            rotationSpeed = 0.1f;
            Vector2 targetPos = t.obj.transform.position - e.transform.position;
            nextAngle = targetPos;
        }
    }
}
