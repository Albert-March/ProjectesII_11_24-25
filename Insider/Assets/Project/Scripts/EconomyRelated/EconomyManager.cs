using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public int economy = 400;
    public Text economyText;
    private int lastEconomy;
    private float scale = 0.8f;

    void Update()
    {
        if (economy > lastEconomy)
        {
            scale = 1.4f;

            if (TutorialManager.instance != null && TutorialManager.instance.IsWaitingForCoinStep())
            {
                TutorialManager.instance.OnCoinCollected();
            }
        }

        if (economy != lastEconomy)
        {
            economyText.text = economy.ToString();
            lastEconomy = economy;
        }

        scale = Mathf.Lerp(scale, 0.8f, Time.deltaTime * 10);
        economyText.transform.localScale = Vector3.one * scale;
    }
}