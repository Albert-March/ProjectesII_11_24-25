using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastCam : MonoBehaviour
{
    public EconomyManager Money;
    public ParasiteManager Health;
    int currentSpeed = 3;
    public Text speedText;
    private bool GodMode = false; 
    private void Start()
    {
        currentSpeed = (int)Time.timeScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            GodMode = !GodMode;
        }
        if (Input.GetKeyDown(KeyCode.K) && GodMode)
        {
            Money.economy += 100;
        }
        if (Input.GetKeyDown(KeyCode.L) && GodMode)
        {
            Health.parasiteHealth += 100;
        }

        if (Input.GetButtonDown("SpeedUp")) 
        {
            if (currentSpeed < 4 || GodMode)
                currentSpeed++;
        }
        if (Input.GetButtonDown("SpeedDown"))
        {
            if (currentSpeed > 3)
                currentSpeed--;
        }

        Time.timeScale = currentSpeed;
        speedText.text = "x"+(currentSpeed - 2).ToString();
    }
}
