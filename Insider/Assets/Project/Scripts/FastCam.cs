using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCam : MonoBehaviour
{
    int currentSpeed = 1;
    void Update()
    {
        if (Input.GetButtonDown("SpeedUp")) 
        {
            if (currentSpeed < 3)
                currentSpeed++;
        }
        if (Input.GetButtonDown("SpeedDown"))
        {
            if (currentSpeed > 1)
                currentSpeed--;
        }

        Time.timeScale = currentSpeed;
    }
}
