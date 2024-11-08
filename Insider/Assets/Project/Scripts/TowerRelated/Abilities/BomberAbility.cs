using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAbility : AbilityManager
{
    public override void Ability(Tower t)
    {
        t.projectileSpeed++;
        t.damage++;
        t.range++;
    }
}
