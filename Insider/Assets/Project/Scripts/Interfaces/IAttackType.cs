using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackType
{
    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType , TargetingManager targetManager);
}
