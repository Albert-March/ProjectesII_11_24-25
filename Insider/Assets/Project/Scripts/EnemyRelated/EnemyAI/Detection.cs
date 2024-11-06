using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public List<Transform> Obstacles;
    public Target t;

    public void Move(Enemy e, Target t)
    {
        if (Obstacles.Count == 0)
        {
            Streight(e, t);
        }
        else 
        {
            Vector3 ePoint = e.transform.position;
            Vector3 tPoint = t.obj.transform.position;
            Vector3 diff = tPoint - ePoint;
            RaycastHit2D hit = Physics2D.Raycast(ePoint, diff, diff.magnitude);
            Debug.DrawRay(ePoint, diff, Color.green);

            if (hit.transform.tag == "Wall") 
            {
                e.transform.position = Vector3.MoveTowards(e.transform.position, CalculateDistanceOfCenterPoint(), e.movSpeed * Time.deltaTime);
            }

        }
    }

    void Streight(Enemy e, Target t) 
    {
        e.transform.position = Vector3.MoveTowards(e.transform.position, t.obj.transform.position, e.movSpeed * Time.deltaTime);
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

    Vector2 CalculateDistanceOfCenterPoint() 
    {
        Vector2 heading = calculateCentroid(Obstacles) - new Vector2(transform.position.x, transform.position.y);
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

