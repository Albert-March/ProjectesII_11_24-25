using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public int economy = 400;
	public Text economyText;


	void Update()
	{
		economyText.text = economy.ToString();
	}
}
