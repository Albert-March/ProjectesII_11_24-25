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
    public Text waveNum;
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
            time += Time.deltaTime;
            timeToAct = spawnManager.currentState.delay / 4;
            waveNum.text = (spawnManager.currentWaveIndex + 1).ToString();

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
