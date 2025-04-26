using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public float time;
	public float timeToAct;
	public CanvasGroup img;
	public Text I_waveTop;
	public Text W_waveTop;
	public Text F_waveTop;
	public Text I_waveMidle;
	public Text W_waveMidle;
	public Text F_waveMidle;
	public Text I_waveBot;
	public Text W_waveBot;
	public Text F_waveBot;

	public bool created;

	void Start()
	{

	}

	void Update()
	{
		if (spawnManager.isInDelayState)
		{
			string s = spawnManager.currentState.stateName;
			time += Time.deltaTime;
			timeToAct = spawnManager.currentState.delay / 4;

			switch (s)
			{
				case "initial":
					//I_waveTop.text = "They are getting closer...";
					I_waveTop.fontSize = 25;

					//I_waveMidle.text = "Brace yourself...";
					I_waveMidle.fontSize = 70;

					//I_waveBot.text = "Spread and consume everything in your path";
					I_waveBot.fontSize = 45;
					break;
				case "wave":
					//W_waveTop.text = "The swarm grows stronger!";
					W_waveTop.fontSize = 25;

					//W_waveMidle.text = "Wave";
					W_waveMidle.fontSize = 150;

					W_waveBot.text = (spawnManager.currentWaveIndex + 1).ToString();
					W_waveBot.fontSize = 150;

					break;
				case "finish":
					//F_waveTop.text = "Nothing remains...";
					F_waveBot.fontSize = 25;

					//F_waveMidle.text = "You have consumed all";
					F_waveBot.fontSize = 70;

					//F_waveBot.text = "in your path";
					F_waveBot.fontSize = 60;
					break;

			}




			if (time < timeToAct)
			{
				created = true;
			}

			if (time > spawnManager.currentState.delay - timeToAct)
			{
				created = false;
			}


			if (created)
			{
				img.alpha += Time.deltaTime / timeToAct;
			}
			else
			{
				img.alpha -= Time.deltaTime / timeToAct;
			}
		}
		else
		{
			time = 0;
		}
	}
}