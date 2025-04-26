using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public int parasiteHealth;
    public Text parasiteText;
    private int lastHealth;
    private float scale = 1f;
    public void Start()
    {
        parasiteText.text = "HP: " + parasiteHealth.ToString();
    }
    public void Update()
    {
        if (parasiteHealth > lastHealth)
        {
            scale = 1.5f;
        }
        if (parasiteHealth != lastHealth)
        {
            parasiteText.text = "HP: " + parasiteHealth.ToString();
            lastHealth = parasiteHealth;
        }
        if (parasiteHealth < 0)
        {
            S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
            transition.CallPass("DeathScreen");
        }

        scale = Mathf.Lerp(scale, 1f, Time.deltaTime * 10);
        parasiteText.transform.localScale = Vector3.one * scale;
    }
    public void Damage(float amount)
	{
		parasiteHealth -= (int)amount;
    }
}
