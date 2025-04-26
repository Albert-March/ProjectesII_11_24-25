using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
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

    private Image highlightImage;
    private Color baseHighlightColor;
    private Transform currentTarget = null;

    void Awake()
    {
        instance = this;
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
        Cannoner.SetActive(false);
        Bopper.SetActive(false);
        Leiser.SetActive(false);
        ShowStep(0);
    }

    public void Yes()
    {
        buttonPressed = true;
        tutorialEnabled = true;
    }

    public void No()
    {
        buttonPressed = true;
        tutorialEnabled = false;
    }

    public void NextStep()
    {
        nextStep = true;
    }

    void Update()
    {
        if (buttonPressed && tutorialEnabled)
        {

            if (currentTarget != null)
            {
                highlightBox.transform.position = currentTarget.position;
            }
            if (highlightBox.activeSelf)
            {
                float alpha = Mathf.PingPong(Time.unscaledTime * 2f, 0.5f) + 0.5f;
                Color color = baseHighlightColor;
                color.a = alpha;
                highlightImage.color = color;
            }

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
    }

    public void ShowStep(int index)
    {
        currentStep = index;
        tutorialPanel.SetActive(true);

        switch (index)
        {
            case 0:
                tutorialText.text = "¿Deseas seguir el camino de la infestación?";
                highlightBox.SetActive(false);
                currentTarget = null;
                break;

            case 1:
                NextStep_Button.SetActive(true);
                tutorialText.text = "Ah... por fin despiertas. Permíteme guiarte. Aprenderás a sobrevivir... y a conquistar este cuerpo.";
                highlightBox.SetActive(false);
                currentTarget = null;
                break;

            case 2:
                tutorialText.text = "Mírate. Un pequeño intruso, pero con un gran propósito: defenderte de esos molestos anticuerpos... y tomar el control desde dentro.";
                currentTarget = step2;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 3:
                NextStep_Button.SetActive(false);
                tutorialText.text = "Primero, únete a las infecciones latentes del cuerpo. Úsalas a tu antojo... conviértelas en armas.";
                SP7.enabled = true;
                Cannoner.SetActive(true);
                currentTarget = step3p1;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 4:
                tutorialText.text = "Genera esta mutación. Será tu primera expansión dentro del huésped.";
                currentTarget = step3p2;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 5:
                NextStep_Button.SetActive(true);
                tutorialText.text = "Pero cuidado... para expandir tu poder necesitarás mutaciones. Evoluciona... adáptate...";
                currentTarget = step5;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 6:
                NextStep_Button.SetActive(false);
                WavePanel.SetActive(true);
                tutorialText.text = "Cuando estés listo, lanza una señal: debilita al cuerpo. Oblígalo a ceder. Solo entonces comenzarás a moldearlo a tu imagen.";
                currentTarget = step4;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                highlightBox.SetActive(true);
                break;

            case 7:
                NextStep_Button.SetActive(true);
                tutorialText.text = "Deshazte de los anticuerpos: sus restos contienen ADN... material precioso que podrás usar para perfeccionar tus infecciones... y nacer más fuerte.";
                currentTarget = step6;
                highlightBox.transform.position = currentTarget.position;
                highlightBox.transform.rotation = Quaternion.identity;
                highlightBox.SetActive(true);
                break;

            case 8:
                tutorialText.text = "Ya has aprendido lo esencial. A partir de ahora... estarás solo. Que la infestación te acompañe... la necesitarás.";
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
