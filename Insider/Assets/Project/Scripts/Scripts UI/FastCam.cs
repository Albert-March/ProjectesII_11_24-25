using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastCam : MonoBehaviour
{
    int currentSpeed = 3;
    public Text speedText;

    private void Start()
    {
        currentSpeed = (int)Time.timeScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("SpeedUp")) 
        {
            if (currentSpeed < 4)
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
