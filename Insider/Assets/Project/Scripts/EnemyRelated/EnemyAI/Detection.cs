using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public List<Transform> Obstacles;
    public Target t;
    public Vector2 targetDirection;
    public float EnemyRotation;

    public Vector2 currentAngle;
    public Vector2 nextAngle;


    public void Start()
    {

    }

    public void Move(Enemy e, Target t)
    {

        MoveUP(e);
        transform.up = RotateOvertime(nextAngle);
        Vector2 frontAngle = Quaternion.Euler(0, 0, 0) * transform.up;
        Vector2 rightAngle = Quaternion.Euler(0, 0, 30) * transform.up;
        Vector2 leftAngle = Quaternion.Euler(0, 0, -30) * transform.up;

        RaycastHit2D front = Physics2D.Raycast(e.transform.position, frontAngle, frontAngle.magnitude);
        RaycastHit2D right = Physics2D.Raycast(e.transform.position, rightAngle, rightAngle.magnitude);
        RaycastHit2D left = Physics2D.Raycast(e.transform.position, leftAngle, rightAngle.magnitude);

        Debug.DrawRay(e.transform.position, frontAngle*10, Color.green);
        Debug.DrawRay(e.transform.position, rightAngle, Color.red);
        Debug.DrawRay(e.transform.position, leftAngle, Color.blue);

        if (Obstacles.Count == 0)
        {
            Streight(e, t);
        }
        else
        {
            if (right.transform.tag == "Wall") 
            {
                nextAngle = Quaternion.Euler(0, 0, -30) * transform.up;
            }
            else
            {
                Streight(e, t);
            }
            if (left.transform.tag == "Wall")
            {
                nextAngle = Quaternion.Euler(0, 0, 30) * transform.up;
            }
            else 
            {
                Streight(e, t);
            }

        }
    }

    void MoveUP(Enemy e) 
    {
        e.transform.position += e.transform.up * Time.deltaTime * e.movSpeed;
    }

    Vector2 RotateOvertime(Vector2 nA)
    {
        Vector2 cA = transform.up;

        if (cA != nA)
        {
            if (cA.magnitude < nA.magnitude)
            {
                return cA + nA / 1000;
            }
            else 
            { 
                return cA + nA / 1000; 
            }
        }
        else 
        {
            return cA;
        }
    }

    void Streight(Enemy e, Target t) 
    {
        nextAngle = t.obj.transform.position - transform.position;
    }



    public void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.transform.tag == "Wall") 
        {
            Obstacles.Add(col.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Wall")
        {
            Obstacles.Remove(col.transform);
        }
    }

    Vector2 CalculateDirectionOfCenterPoint() 
    {
        Vector2 heading = calculateCentroid(Obstacles) - (Vector2)transform.position;
        float distance = heading.magnitude;
        Vector2 centricPointDistance = heading / distance;

        return centricPointDistance;
    }

    Vector2 calculateCentroid(List<Transform> centerPoints)
    {
        Vector2 centroid = new Vector2(0, 0);
        var numPoints = centerPoints.Count;
        foreach (Transform point in centerPoints)
        {
            centroid += new Vector2(point.position.x, point.position.y);
        }

        centroid /= numPoints;

        return centroid;
    }
}

