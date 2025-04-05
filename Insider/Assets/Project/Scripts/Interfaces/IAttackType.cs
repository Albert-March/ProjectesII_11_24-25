using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackType
{
    public void Attack(Enemy e, Animator anim, AudioManager audio);
}
