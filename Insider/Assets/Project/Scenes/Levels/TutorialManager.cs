using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("Tutorial Control")]
    public bool tutorialEnabled = true;

    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public Text tutorialText;
    public GameObject highlightBox;
    public Button nextButton;

    [Header("Tutorial Targets")]
    public Transform buttonWave;
    public Transform enemyExample;
    public Transform towerPoint;
    public Transform moneyText;
    public Transform lifeText;

    public Spawner spRef;

    private int currentStep = 0;
    private bool isPaused = false;
    private bool towerPlaced = false;
    private bool coinCollected = false;

    private Image highlightImage;
    private Color baseHighlightColor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!tutorialEnabled)
        {
            tutorialPanel.SetActive(false);
            highlightBox.SetActive(false);
            return;
        }

        highlightImage = highlightBox.GetComponent<Image>();
        baseHighlightColor = highlightImage.color;

        ShowStep(0);
    }

    void Update()
    {
     
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
                if (!spRef.waitingForNextWave)
                {
                    NextStep();
                }
                break;

            case 1:
                if (GameObject.FindWithTag("Enemy") != null)
                {
                    NextStep();
                }
                break;

            case 2:
                if (towerPlaced)
                {
                    NextStep();
                }
                break;

            case 3:
                if (coinCollected)
                {
                    NextStep();
                }
                break;
        }
    }

    public void ShowStep(int index)
    {
        currentStep = index;
        tutorialPanel.SetActive(true);
        highlightBox.SetActive(true);

       
        if (index == 0)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;

        isPaused = true;

        
        switch (index)
        {
            case 0:
                tutorialText.text = "Click here to start the first wave.";
                highlightBox.transform.position = Camera.main.WorldToScreenPoint(buttonWave.position);
                break;

            case 1:
                tutorialText.text = "These are the enemies. Stop them!";
                highlightBox.transform.position = Camera.main.WorldToScreenPoint(enemyExample.position);
                break;

            case 2:
                tutorialText.text = "Place a tower here to defend!";
                highlightBox.transform.position = Camera.main.WorldToScreenPoint(towerPoint.position);
                break;

            case 3:
                tutorialText.text = "This is a coin. You earn them by killing enemies and use them to build or upgrade towers.";
                highlightBox.transform.position = Camera.main.WorldToScreenPoint(moneyText.position);
                break;

            case 4:
                tutorialText.text = "This is your life. If it reaches 0, you lose!";
                highlightBox.transform.position = Camera.main.WorldToScreenPoint(lifeText.position);
                break;
        }
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        highlightBox.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnTowerPlaced()
    {
        towerPlaced = true;
    }

    public void OnCoinCollected()
    {
        coinCollected = true;
    }

    public bool IsWaitingForCoinStep()
    {
        return currentStep == 3;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep <= 4)
            ShowStep(currentStep);
        else
            HideTutorial();
    }
}