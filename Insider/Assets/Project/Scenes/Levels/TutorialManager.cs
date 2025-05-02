using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("Tutorial Control")]
    public bool buttonPressed = false;
    public bool tutorialEnabled = true;
    public bool nextStep = false;

    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public Text tutorialText;
    public GameObject highlightBox;
    public GameObject Si_Button;
    public GameObject No_Button;
    public GameObject NextStep_Button;

    public LocalizedString tutorialStep0;
    public LocalizedString tutorialStep1;
    public LocalizedString tutorialStep2;
    public LocalizedString tutorialStep3;
    public LocalizedString tutorialStep4;
    public LocalizedString tutorialStep5;
    public LocalizedString tutorialStep6;
    public LocalizedString tutorialStep7;
    public LocalizedString tutorialStep8;

	[Header("Tutorial Targets")]
    public Transform step2;
    public Transform step3p1;
    public Transform step3p2;
    public Transform step4;
    public Transform step5;
    public Transform step6;

    [Header("Additional Needs Step 3 and 4")]
    public Button SP1;
    public Button SP2;
    public Button SP3;
    public Button SP4;
    public Button SP5;
    public Button SP6;
    public Button SP7;
    public DinamicTowerSetting spotFirstTower;
    public PanelVisibilityController dinamicPanel;
    public GameObject Cannoner;
    public GameObject Bopper;
    public GameObject Leiser;

    [Header("Additional Needs Step 6")]
    public GameObject WavePanel;
    public Spawner spawner;

    public int currentStep = 0;

	private Vector3 originalScale;
	public float pulseSpeed = 2f;
	public float pulseAmount = 0.1f;

	private Image highlightImage;
    private Color baseHighlightColor;
    private Transform currentTarget = null;
    public AudioManager audioManager;

    void Awake()
    {
        instance = this;
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

    void Start()
    {
        highlightImage = highlightBox.GetComponent<Image>();
        baseHighlightColor = highlightImage.color;
		originalScale = NextStep_Button.transform.localScale;

		NextStep_Button.SetActive(false);
        SP1.enabled = false;
        SP2.enabled = false;
        SP3.enabled = false;
        SP4.enabled = false;
        SP5.enabled = false;
        SP6.enabled = false;
        SP7.enabled = false;
        Cannoner.SetActive(false);
        Bopper.SetActive(false);
        Leiser.SetActive(false);
        ShowStep(0);
    }

    public void Yes()
    {
        audioManager.PlaySFX(3, 0.2f);
        buttonPressed = true;
        tutorialEnabled = true;
    }

    public void No()
    {
        audioManager.PlaySFX(3, 0.2f);
        buttonPressed = true;
        tutorialEnabled = false;

		SP1.enabled = true;
		SP2.enabled = true;
		SP3.enabled = true;
		SP4.enabled = true;
		SP5.enabled = true;
		SP6.enabled = true;
		SP7.enabled = true;
		Cannoner.SetActive(true);
		Bopper.SetActive(true);
		Leiser.SetActive(true);
		WavePanel.SetActive(true);
	}

    public void NextStep()
    {
        audioManager.PlaySFX(3, 0.2f);
        nextStep = true;
    }

    void Update()
    {
        if (buttonPressed && tutorialEnabled)
        {
            if (currentTarget != null)
            {
                // Solo actualizar la posición
                highlightBox.transform.position = currentTarget.position;
            }

            if (highlightBox.activeSelf)
            {
                // Solo actualizar el parpadeo
                float alpha = Mathf.PingPong(Time.unscaledTime * 2f, 0.5f) + 0.5f;
                Color color = baseHighlightColor;
                color.a = alpha;
                highlightImage.color = color;
            }

            // NO LLAMAR ShowStep() aquí
            // Solo gestionar cuándo cambiar de paso
            switch (currentStep)
            {
                case 0:
                    if (buttonPressed)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 1));
                        Si_Button.SetActive(false);
                        No_Button.SetActive(false);
                    }
                    break;

                case 1:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 2));
                    }
                    break;

                case 2:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 3));
                    }
                    break;

                case 3:
                    if (dinamicPanel.open)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(0.1f, 4));
                    }
                    break;

                case 4:
                    if (!dinamicPanel.open)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(0.1f, 3));
                    }
                    if (spotFirstTower.spawnTower)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 5));
                    }
                    break;

                case 5:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 6));
                    }
                    break;

                case 6:
                    if (spawner.waitingForNextWave == false)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 7));
                    }
                    break;

                case 7:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 8));
                    }
                    break;

                case 8:
                    if (nextStep)
                    {
                        nextStep = false;
                        tutorialEnabled = false;
                        HideTutorial();
                    }
                    break;
            }
        }
        else if (!tutorialEnabled)
        {
            tutorialPanel.SetActive(false);
            highlightBox.SetActive(false);
        }

        float scaleFactor = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        NextStep_Button.transform.localScale = originalScale * scaleFactor;
    }


	public void ShowStep(int index)
    {
        currentStep = index;
        tutorialPanel.SetActive(true);

		switch (index)
		{
			case 0:
				tutorialText.text = tutorialStep0.GetLocalizedString();
				highlightBox.SetActive(false);
				currentTarget = null;
				break;

			case 1:
				NextStep_Button.SetActive(true);
				tutorialText.text = tutorialStep1.GetLocalizedString();
				highlightBox.SetActive(false);
				currentTarget = null;
				break;

			case 2:
				tutorialText.text = tutorialStep2.GetLocalizedString();
				currentTarget = step2;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.identity;
				highlightBox.SetActive(true);
				break;

			case 3:
				NextStep_Button.SetActive(false);
				tutorialText.text = tutorialStep3.GetLocalizedString();
				SP7.enabled = true;
				Cannoner.SetActive(true);
				currentTarget = step3p1;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.identity;
				highlightBox.SetActive(true);
				break;

			case 4:
				tutorialText.text = tutorialStep4.GetLocalizedString();
				currentTarget = step3p2;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.identity;
				highlightBox.SetActive(true);
				break;

			case 5:
				NextStep_Button.SetActive(true);
				tutorialText.text = tutorialStep5.GetLocalizedString();
				currentTarget = step5;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.identity;
				highlightBox.SetActive(true);
				break;

			case 6:
				NextStep_Button.SetActive(false);
				WavePanel.SetActive(true);
				tutorialText.text = tutorialStep6.GetLocalizedString();
				currentTarget = step4;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				highlightBox.SetActive(true);
				break;

			case 7:
				NextStep_Button.SetActive(true);
				tutorialText.text = tutorialStep7.GetLocalizedString();
				currentTarget = step6;
				highlightBox.transform.position = currentTarget.position;
				highlightBox.transform.rotation = Quaternion.identity;
				highlightBox.SetActive(true);
				break;

			case 8:
				tutorialText.text = tutorialStep8.GetLocalizedString();
				currentTarget = null;
				SP1.enabled = true;
				SP2.enabled = true;
				SP3.enabled = true;
				SP4.enabled = true;
				SP5.enabled = true;
				SP6.enabled = true;
				SP7.enabled = true;
				Cannoner.SetActive(true);
				Bopper.SetActive(true);
				Leiser.SetActive(true);
				break;
		}
	}


    IEnumerator DelayShowStep(float delay, int stepToShow)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowStep(stepToShow);
    }

	void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        highlightBox.SetActive(false);
    }
}
