using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public float parasiteHealth;
    public Text parasiteText;

    public void Start()
    {
        parasiteText.text = "HP: " + parasiteHealth.ToString();
    }
    public void Damage(float amount)
	{
		parasiteHealth -= amount;
        if (parasiteHealth < 0)
		{
            SceneManager.LoadScene("DeathScreen");
        }
        parasiteText.text = "HP: " + parasiteHealth.ToString();
    }
}
