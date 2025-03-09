using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    private int currentStep = 0;
    private List<string> tutorialSteps = new List<string>
    {
        "How to play: Insider\r\n\nWelcome to Insider, a game where you play as a parasite on a mission to invade the body and prevent antibodies from healing it. To survive, you'll need to build strategic defenses and upgrade your army of infections.",
        "Objective of the game:\r\n\nYour mission is to parasitize all the organs and reach the brain before it is eliminated by the antibodies. To do this, you must:\r\n\nEliminate waves of antibodies.\r\n\nBuild biological defenses such as viruses, fungi and bacteria.\r\n\nUpgrade your units to make them more effective.\r\n\nManage your economy well to resist until the end.",
        "Types of Defenses:\r\n\nYou can deploy three types of biological defenses, each with its own strengths:\r\n\nViruses:\r\n\nFungi:\r\n\nBacteria:\r\n\nEach tower can be upgraded with money.",
        "Key Mechanics:\r\n\nEarning Money:\nEarned by removing antibodies. Spend wisely!\r\n\nUpgrading:\nDon't just build towers, but upgrade the ones you already have.\r\n\nParasite Resistance:\nIf antibodies get to it, you'll lose health. Don't let that happen!",
        "Can you make it to the brain before you're eliminated? Good luck, parasite!"
    };

    void Start()
    {
        tutorialText.text = tutorialSteps[currentStep];
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Count)
        {
            tutorialText.text = tutorialSteps[currentStep];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
