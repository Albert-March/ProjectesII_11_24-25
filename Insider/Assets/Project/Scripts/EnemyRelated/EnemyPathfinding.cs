using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour 
{

    private float distance;
    public float distanceBetween;

    public void MoveTowardsTarget(Enemy enemy, Target t)
    {
        distance = Vector2.Distance(enemy.transform.position, t.obj.transform.position);
        Vector2 direction = t.obj.transform.position - enemy.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (distance > distanceBetween)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, t.obj.transform.position, enemy.GetComponent<Enemy>().movSpeed * Time.deltaTime);
            enemy.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }

}