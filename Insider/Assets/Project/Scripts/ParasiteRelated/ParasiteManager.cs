using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParasiteManager : MonoBehaviour, IDamage
{
	public int parasiteHealth;
	public Text parasiteText;
	public Text parasiteTextMoney;
	private int lastHealth;
	private float scale = 1f;
	private Vector3 originalScale;

	public LocalizedString hpTable;

	//DamageEffect
	public float intensity = 0;
	public GameObject postProcessObject;
	private Volume _volume;
	private Vignette _vignette;

	private void Start()
	{
		lastHealth = parasiteHealth;
		UpdateHPText();
		originalScale = parasiteTextMoney.transform.localScale;

		// Obtener el PostProcessVolume del GameObject
		_volume = postProcessObject.GetComponent<Volume>();
		if (_volume != null && _volume.profile.TryGet(out _vignette))
		{
			_vignette.active = false;
		}
		else
		{
			Debug.LogWarning("No se encontró el efecto Vignette en el perfil.");
		}


	}

	private void Update()
	{
		if (parasiteHealth != lastHealth)
		{
			scale = 1.5f;
			lastHealth = parasiteHealth;
			StartCoroutine(TakeDamageEffect());
		}
		UpdateHPText();

		scale = Mathf.Lerp(scale, 1f, Time.deltaTime * 10);
        parasiteTextMoney.transform.localScale = originalScale * scale;

		if (parasiteHealth <= 0)
		{
			S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
			transition.CallPass("DeathScreen");
		}
		parasiteTextMoney.text = parasiteHealth.ToString();

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

	private IEnumerator TakeDamageEffect()
	{
		intensity = 0.4f;

		_vignette.active = true;
		_vignette.intensity.Override(0.4f);

		yield return new WaitForSeconds(0.4f);

		while(intensity > 0)
		{
			intensity -= 0.01f;

			if(intensity < 0) intensity = 0;

			_vignette.intensity.Override(intensity);

			yield return new WaitForSeconds(0.1f);
		}

		_vignette.active = false;

		yield break;
	}
}
