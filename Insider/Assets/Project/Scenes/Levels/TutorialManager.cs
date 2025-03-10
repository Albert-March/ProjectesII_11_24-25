using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;

    private int currentStep = 0;

    private void Start()
    {
        currentStep = 0;
    }

    void Update()
    {
        if (currentStep == 0)
        {
            P1.SetActive(true);
            P2.SetActive(false);
        }
        else if (currentStep == 1)
        {
            P1.SetActive(false);
            P2.SetActive(true);
        }
        else
        {
            S_LevelLoader transition = GameObject.Find("LevelLoader").GetComponent<S_LevelLoader>();
            transition.CallPass("MainMenu");
        }
    }

    public void NextStep()
    {
        currentStep++;
    }
}
