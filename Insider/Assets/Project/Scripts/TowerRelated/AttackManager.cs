using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    public AttackManager towerAttackBehaviour;
    public abstract void  Attack(Enemy e);
    public void Set(int id)
    {
        switch (id) 
        {
            case 0:
                towerAttackBehaviour.AddComponent<A_Virus1>();
                break;
            default:
                towerAttackBehaviour = null;
                break;
        }
    }
}