using System;
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
            case 2:
                attackType = gameObject.AddComponent<A_Bacteri1>();
                break;
            case 3:
                attackType = gameObject.AddComponent<A_Bacteri2>();
                break;
            case 4:
                attackType = gameObject.AddComponent<A_Fong1>();
                break;
            case 5:
                attackType = gameObject.AddComponent<A_Fong2>();
                break;
            default:
                attackType = null;
                break;
        }
    }
}