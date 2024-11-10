using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public float parasiteHealth;

	public void Damage(float amount)
	{
		parasiteHealth -= amount;
	}
}
