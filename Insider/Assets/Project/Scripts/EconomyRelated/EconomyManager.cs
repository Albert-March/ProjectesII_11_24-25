using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public int economy = 100;
	public TextMeshProUGUI economyText;


	void Update()
	{
		economyText.text = "Economy: " + economy.ToString();
	}
}
