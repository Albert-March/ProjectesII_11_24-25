using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ChangeLenguage : MonoBehaviour
{
	AudioManager audioManager;
	public Image EN_Flag;
	public Image CA_Flag;
	public Image ES_Flag;

	private bool active = false;
	public int lenguageId;

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	//IEnumerator Start()
	//{
	//	yield return LocalizationSettings.InitializationOperation;
	//	LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales
	//		.Find(locale => locale.Identifier.Code == "en");
	//}

	void Update()
	{
		switch (lenguageId) { 
			case 0:
				EN_Flag.color = Color.white;
				CA_Flag.color = Color.gray;
				ES_Flag.color = Color.gray;
				break;
			case 1:
				EN_Flag.color = Color.gray;
				CA_Flag.color = Color.white;
				ES_Flag.color = Color.gray;
				break;
			case 2:
				EN_Flag.color = Color.gray;
				CA_Flag.color = Color.gray;
				ES_Flag.color = Color.white;
				break;
		}
	}
	public void ChangeLenguages(int localeId)
    {
		lenguageId = localeId;
		audioManager.PlaySFX(4, 0.2f);
		if (active) { return; }
        StartCoroutine(SetLenguage(localeId));
    }

    IEnumerator SetLenguage(int _localeId)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
        active = false;

	}
}
