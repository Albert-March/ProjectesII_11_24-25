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
    public bool alredyinCorrutine = false;

    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public Text tutorialText;
    public GameObject highlightBox;
    public Sprite Select;
    public Sprite Highlight;
    public GameObject Si_Button;
    public GameObject No_Button;
    public GameObject NextStep_Button;

    [Header("Localization")]

    public LocalizedString tutorialStep0;
    public LocalizedString tutorialStep1;
    public LocalizedString tutorialStep2;
    public LocalizedString tutorialStep3;
    public LocalizedString tutorialStep4;
    public LocalizedString tutorialStep5;
    public LocalizedString tutorialStep6;
    public LocalizedString tutorialStep7;
    public LocalizedString tutorialStep8;
    public LocalizedString tutorialStep9;
    public LocalizedString tutorialStep10;
    public LocalizedString tutorialStep11;
    public LocalizedString tutorialStep12;
    public LocalizedString tutorialStep13;
    public LocalizedString tutorialStep14;
    public LocalizedString tutorialStep15;
    public LocalizedString tutorialStep16;
    public LocalizedString tutorialStep17;

	[Header("Tutorial Targets")]
    public Transform TutorialTowerSpot1;
    public Transform TutorialTowerSpot2;

    public Transform TutorialCannoner;
    public Transform TutorialLeiser;
    public Transform TutorialUpgrade;

    public Transform TutorialNextWave;

    public Transform TutorialSpotEconomy;
    public Transform TutorialADNEconomy;
    public Transform TutorialHealth;

    [Header("Additional Needs")]
    public Button SP1;
    public Button SP2;
    public Button SP3;
    public Button SP4;
    public Button SP5;
    public Button SP6;
    public Button SP7;
    public Button SPBG;
    public DinamicTowerSetting spotFirstTower;
    public DinamicTowerSetting spotSecondTower;
    public PanelVisibilityController dinamicPanel;
    public GameObject Cannoner;
    public GameObject Bopper;
    public GameObject Leiser;

    public GameObject Player;

    public Button Upgrade1_1;
    public Button Upgrade1_2;
    public Button Upgrade2_1;
    public Button Upgrade2_2;

    public GameObject WavePanel;
    public Button DropdownButton;
    public Spawner spawner;

    [Header("Tutorial Needs")]


    public Image Show;

    public Sprite imageSupport;
    public Sprite imageSinglefire;
    public Sprite imageMultitarget;


    public int currentStep = 0;

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

        NextStep_Button.SetActive(false);
        SP1.enabled = false;
        SP2.enabled = false;
        SP3.enabled = false;
        SP4.enabled = false;
        SP5.enabled = false;
        SP6.enabled = false;
        SP7.enabled = false;
        SPBG.enabled = false;

        Show.enabled = false;

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
                    if (dinamicPanel.open)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 3));
                        SP7.enabled = false;
                    }
                    break;

                case 3:
                    if (spotFirstTower.spawnTower)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 4));
                        dinamicPanel.ClosePanel();
                        DropdownButton.enabled = false;
                    }
                    break;

                case 4:
                    if (spawner.waitingForNextWave == false)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(6f, 5));
                        WavePanel.SetActive(false);
                    }
                    break;

                case 5:
                    if (spawner.waitingForNextWave == true)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 6));
                    }
                    break;

                case 6:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 7));
                    }
                    break;

                case 7:
                    if (dinamicPanel.open)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 8));
                        SP3.enabled = false;
                    }
                    break;

                case 8:
                    if (spotSecondTower.spawnTower)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 9));
                        dinamicPanel.ClosePanel();
                    }
                    break;

                case 9:
                    if (spawner.waitingForNextWave == false)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(19f, 10));
                        WavePanel.SetActive(false);
                    }
                    break;
                case 10:
                    if (nextStep)
                    {
                        nextStep = false;
                        
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 11));
                        Player.GetComponent<ParasiteManager>().parasiteHealth = 100;
                    }
                    break;

                case 11:
                    if(dinamicPanel.open)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 12));
                        SP3.enabled = false;
                    }
                    break;
                case 12:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 13));
                    }
                    break;
                case 13:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(0.5f, 14));
                    }
                    break;
                case 14:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(0.5f, 15));
                    }
                    break;
                case 15:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(0.5f, 16));
                    }
                    break;
                case 16:
                    if (spotSecondTower.levelUp2 == true)
                    {
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 17));
                    }
                    break;
                case 17:
                    if (nextStep)
                    {
                        nextStep = false;
                        HideTutorial();
                        StartCoroutine(DelayShowStep(1f, 18));
                    }
                    break;
            }
        }
        else if (!tutorialEnabled)
        {
            tutorialPanel.SetActive(false);
            highlightBox.SetActive(false);
        }
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
                NextStep_Button.SetActive(false);
                tutorialText.text = tutorialStep2.GetLocalizedString();
                SP7.enabled = true;
                Cannoner.SetActive(true);
                currentTarget = TutorialTowerSpot1;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

			case 3:
                tutorialText.text = tutorialStep3.GetLocalizedString();
                currentTarget = TutorialCannoner;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

			case 4:
                NextStep_Button.SetActive(false);
                WavePanel.SetActive(true);
                tutorialText.text = tutorialStep4.GetLocalizedString();
                currentTarget = TutorialNextWave;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                highlightBox.SetActive(true);
                break;

			case 5:
                NextStep_Button.SetActive(false);
                tutorialText.text = tutorialStep5.GetLocalizedString();
                currentTarget = TutorialADNEconomy;
                highlightBox.GetComponent<Image>().sprite = Highlight;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);

                break;

			case 6:
                NextStep_Button.SetActive(true);
                WavePanel.SetActive(false);
                tutorialText.text = tutorialStep6.GetLocalizedString();
                currentTarget = TutorialSpotEconomy;
                highlightBox.GetComponent<Image>().sprite = Highlight;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);

                break;

			case 7:
                SP3.enabled = true;
                NextStep_Button.SetActive(false);
                tutorialText.text = tutorialStep7.GetLocalizedString();
                currentTarget = TutorialTowerSpot2;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;
            
            case 8:
                Cannoner.GetComponent<Button>().enabled = false;
                Leiser.SetActive(true);
                tutorialText.text = tutorialStep8.GetLocalizedString();
                currentTarget = TutorialLeiser;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 9:
                WavePanel.SetActive(true);
                WavePanel.GetComponent<WavesInformation>().OpenPanel();
                tutorialText.text = tutorialStep9.GetLocalizedString();
                currentTarget = TutorialNextWave;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                highlightBox.SetActive(true);
                break;

            case 10:
                NextStep_Button.SetActive(true);
                tutorialText.text = tutorialStep10.GetLocalizedString();
                currentTarget = TutorialHealth;
                highlightBox.GetComponent<Image>().sprite = Highlight;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                highlightBox.SetActive(true);
                break;

            case 11:

                SP3.enabled = true;
                NextStep_Button.SetActive(false);
                tutorialText.text = tutorialStep11.GetLocalizedString();
                currentTarget = TutorialTowerSpot2;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 12:
                Upgrade1_1.enabled = false;
                Upgrade1_2.enabled = false;
                Upgrade2_1.enabled = false;
                Upgrade2_2.enabled = false;
                NextStep_Button.SetActive(true);
                tutorialText.text = tutorialStep12.GetLocalizedString();
                currentTarget = TutorialTowerSpot2;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(false);
                break;

            case 13:
                Show.enabled = true;
                Show.sprite = imageSupport;
                tutorialText.text = tutorialStep13.GetLocalizedString();
                break;

            case 14:
                Show.sprite = imageSinglefire;
                tutorialText.text = tutorialStep14.GetLocalizedString();
                break;

            case 15:
                Show.sprite = imageMultitarget;
                tutorialText.text = tutorialStep15.GetLocalizedString();
                break;

            case 16:
                NextStep_Button.SetActive(false);
                Show.enabled = false;
                Upgrade1_1.enabled = true;
                tutorialText.text = tutorialStep16.GetLocalizedString();
                currentTarget = TutorialUpgrade;
                highlightBox.GetComponent<Image>().sprite = Select;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 17:
                NextStep_Button.SetActive(true);
                tutorialText.text = tutorialStep17.GetLocalizedString();
                currentTarget = TutorialUpgrade;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(false);
                break;
            case 18:
                tutorialEnabled = false;

                currentTarget = null;
				SP1.enabled = true;
				SP2.enabled = true;
				SP3.enabled = true;
				SP4.enabled = true;
				SP5.enabled = true;
				SP6.enabled = true;
				SP7.enabled = true;

                Cannoner.GetComponent<Button>().enabled = false;
                Cannoner.SetActive(true);
				Bopper.SetActive(true);
				Leiser.SetActive(true);

                WavePanel.SetActive(true);
                WavePanel.GetComponent<WavesInformation>().OpenPanel();
                DropdownButton.enabled = true;

                Upgrade1_1.enabled = true;
                Upgrade1_2.enabled = true;
                Upgrade2_1.enabled = true;
                Upgrade2_2.enabled = true;


                break;
		}
	}


    IEnumerator DelayShowStep(float delay, int stepToShow)
    {
        if (alredyinCorrutine) { yield break; }
        else { alredyinCorrutine = true; }
        yield return new WaitForSecondsRealtime(delay);
        alredyinCorrutine = false;
        ShowStep(stepToShow);
    }

	void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        highlightBox.SetActive(false);
    }
}
