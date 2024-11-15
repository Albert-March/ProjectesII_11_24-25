using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : EnemyBehaviour
{
    public override void Behave(Enemy e, Target t)
    {
        e.transform.position += e.transform.up * Time.deltaTime * e.movSpeed;
    }
}
