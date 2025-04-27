using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EconomyManager : MonoBehaviour
{
    public int economy;
	public Text economyText;
	private int lastEconomy;
	private float scaleE = 1f;

	public int towerSpots = 2;
	public Text towerSpotsText;
	private int lasttowerSpots;
	private float scaleT = 1f;

	public bool insufficientEconomy = false;
	public bool insufficientTowerSpots = false;

	private Vector3 originalEconomyPosition;
	private Vector3 originalTowerSpotsPosition;

	public float shakeMagnitude = 1.5f;
	public float shakeSpeed = 25f;
	public float shakeDuration = 1f;

	private float economyShakeTimer = 0f;
	private float towerSpotsShakeTimer = 0f;


	void Start()
	{
		originalEconomyPosition = economyText.transform.localPosition;
		originalTowerSpotsPosition = towerSpotsText.transform.localPosition;
	}

	void Update()
	{
		if (economy > lastEconomy)
		{
			scaleE = 1.4f;
		}

		if(towerSpots > lasttowerSpots)
		{
			scaleT = 1.4f;
		}

		if (economy != lastEconomy)
		{
			economyText.text = economy.ToString();
			lastEconomy = economy;
		}

		if(towerSpots != lasttowerSpots)
		{
			towerSpotsText.text = towerSpots.ToString();
			lasttowerSpots = towerSpots;
		}

		scaleE = Mathf.Lerp(scaleE, 1f, Time.deltaTime * 10);
		scaleT = Mathf.Lerp(scaleT, 1f, Time.deltaTime * 10);
		economyText.transform.localScale = Vector3.one * scaleE;
		towerSpotsText.transform.localScale = Vector3.one * scaleT;


		if (insufficientEconomy)
		{
			economyShakeTimer += Time.deltaTime;

			float sinValue = Mathf.Sin(Time.time * shakeSpeed);
			float xOffset = sinValue * sinValue * shakeMagnitude * (sinValue > 0 ? 1 : -1);
			economyText.transform.localPosition = originalEconomyPosition + new Vector3(xOffset, 0f, 0f);

			if (economyShakeTimer >= shakeDuration)
			{
				insufficientEconomy = false;
				economyShakeTimer = 0f;
				economyText.transform.localPosition = originalEconomyPosition;
			}
		}
		else
		{
			economyText.transform.localPosition = originalEconomyPosition;
		}

		if (insufficientTowerSpots)
		{
			towerSpotsShakeTimer += Time.deltaTime;

			float sinValue = Mathf.Sin(Time.time * shakeSpeed);
			float xOffset = sinValue * sinValue * shakeMagnitude * (sinValue > 0 ? 1 : -1);
			towerSpotsText.transform.localPosition = originalTowerSpotsPosition + new Vector3(xOffset, 0f, 0f);

			if (towerSpotsShakeTimer >= shakeDuration)
			{
				insufficientTowerSpots = false;
				towerSpotsShakeTimer = 0f;
				towerSpotsText.transform.localPosition = originalTowerSpotsPosition;
			}
		}
		else
		{
			towerSpotsText.transform.localPosition = originalTowerSpotsPosition;
		}
	}
}