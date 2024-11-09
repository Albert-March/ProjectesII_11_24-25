using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public IAttackType attackType;

    public void SetAttackType(int id) 
    {

        switch (id)
        {
            case 0:
                attackType = gameObject.AddComponent<A_Virus1>();
                break;
            case 1:
                attackType = gameObject.AddComponent<A_Virus2>();
                break;
            default:
                attackType = null;
                break;
        }
    }
}