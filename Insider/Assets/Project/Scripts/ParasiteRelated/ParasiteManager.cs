using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public float parasiteHealth;

	public void Damage(float amount)
	{
		parasiteHealth -= amount;
		if(parasiteHealth < 0)
		{
            SceneManager.LoadScene("DeathScreen");
        }
	}
}
