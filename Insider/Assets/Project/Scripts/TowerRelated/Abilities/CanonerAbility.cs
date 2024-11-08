using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonerAbility : AbilityManager
{
    public override void Ability(Tower t)
    {
        t.projectileSpeed++;
        t.bulletSize++;
    }
}
