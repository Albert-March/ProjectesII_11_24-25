using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] Transform finaltarget;

    [SerializeField] Transform currentTarget;
    [SerializeField] int currentTargetNum = 0;
    [SerializeField] int chosen;
    [SerializeField] Transform[] path1;
    [SerializeField] Transform[] path2;
    [SerializeField] Transform[] pathList;

}
