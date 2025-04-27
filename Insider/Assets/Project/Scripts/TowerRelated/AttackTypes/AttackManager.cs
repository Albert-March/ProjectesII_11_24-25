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
                attackType = gameObject.AddComponent<Atack_Cannoner>();
                break;
            case 1:
                attackType = gameObject.AddComponent<Attack_Boper>();
                break;
            case 2:
                attackType = gameObject.AddComponent<Attack_Laser>();
                break;
            default:
                attackType = null;
                break;
        }
    }
}