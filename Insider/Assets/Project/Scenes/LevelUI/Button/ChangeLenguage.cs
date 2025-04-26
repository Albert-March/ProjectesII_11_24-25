using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLenguage : MonoBehaviour
{
    private bool active = false;
    public void ChangeLenguages(int localeId)
    {
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
