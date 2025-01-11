using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpawnManager;

public class EnemyTypeManager : MonoBehaviour
{
	public void SetEnemyType(int id)
	{
		switch (id)
		{
			case 0:
				Normal normal = gameObject.AddComponent<Normal>();
				break;
			case 1:
				Splitter splitter = gameObject.AddComponent<Splitter>();
				break;
			case 2:
				Money money = gameObject.AddComponent<Money>();
				break;
			case 3:
				Explosive explosive = gameObject.AddComponent<Explosive>();
				break;
			case 4:
				Normal Diflect = gameObject.AddComponent<Normal>();
				break;
			case 5:
				Normal Runner = gameObject.AddComponent<Normal>();
				break;
		}
	}
}
