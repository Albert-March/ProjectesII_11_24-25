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
	public Text waveTop;
	public Text waveMidle;
	public Text waveBot;

	public bool created;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
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
					waveTop.text = "They are getting closer...";
					waveTop.fontSize = 25;

					waveMidle.text = "Brace yourself...";
					waveMidle.fontSize = 70;

					waveBot.text = "Spread and consume everything in your path";
					waveBot.fontSize = 45;
					break;
				case "wave":
					waveTop.text = "The swarm grows stronger!";
					waveTop.fontSize = 25;

					waveMidle.text = "Wave";
					waveMidle.fontSize = 150;

					waveBot.text = (spawnManager.currentWaveIndex + 1).ToString();
					waveBot.fontSize = 150;

					break;
				case "finish":
					waveTop.text = "Nothing remains...";
					waveBot.fontSize = 25;

					waveMidle.text = "You have consumed all";
					waveBot.fontSize = 70;

					waveBot.text = "in your path";
					waveBot.fontSize = 60;
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