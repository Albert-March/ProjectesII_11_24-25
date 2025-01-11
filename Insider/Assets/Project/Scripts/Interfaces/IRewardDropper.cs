using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardDropper
{
	void SpawnReward(List<Target> path);
}

