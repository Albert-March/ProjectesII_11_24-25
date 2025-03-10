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
    private float scale = 0.45f;
    public void Start()
    {
        parasiteText.text = "HP: " + parasiteHealth.ToString();
    }
    public void Update()
    {
        if (parasiteHealth > lastHealth)
        {
            scale = 0.8f;
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

        scale = Mathf.Lerp(scale, 0.45f, Time.deltaTime * 10);
        parasiteText.transform.localScale = Vector3.one * scale;
    }
    public void Damage(float amount)
	{
		parasiteHealth -= (int)amount;
    }
}
