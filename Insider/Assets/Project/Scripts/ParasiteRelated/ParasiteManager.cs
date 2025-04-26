using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public int parasiteHealth;
	public Text parasiteText;
	private int lastHealth;
	private float scale = 1f;
	private Vector3 originalScale;

	public LocalizedString hpTable;

	private void Start()
	{
		lastHealth = parasiteHealth;
		UpdateHPText();
		originalScale = parasiteText.transform.localScale;
	}

	private void Update()
	{
		if (parasiteHealth != lastHealth)
		{
			scale = 1.5f;
			lastHealth = parasiteHealth;
		}
		UpdateHPText();

		scale = Mathf.Lerp(scale, 1f, Time.deltaTime * 10);
		parasiteText.transform.localScale = originalScale * scale;

		if (parasiteHealth < 0)
		{
			S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
			transition.CallPass("DeathScreen");
		}
	}

	public void Damage(float amount)
	{
		parasiteHealth -= (int)amount;
	}

	private void UpdateHPText()
	{
		hpTable.Arguments = new object[] { parasiteHealth };
		parasiteText.text = hpTable.GetLocalizedString();
	}
}
