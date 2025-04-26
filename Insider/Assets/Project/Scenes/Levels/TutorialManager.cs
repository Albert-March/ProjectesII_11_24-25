using System.Collections;
using Unity.VisualScripting;
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
    public GameObject buttonWave;
    public Transform enemyExample;
    public Transform towerPoint;
    public Transform moneyText;
    public Transform lifeText;
    public SpawnManager spawnManager;
    public Spawner spRef;

    private bool hasClickedNextWave = false;
    private bool towerPlaced = false;
    private bool coinCollected = false;

    public int currentStep = 0;
    public bool isPaused = false;

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
                if (!spRef.waitingForNextWave && hasClickedNextWave)
                {
                    StartCoroutine(DelayEnemiesAppearing(4f));
                    currentStep++;
                }
                break;

            case 2:
                if (towerPlaced)
                {
                    ResumeGame();
                    currentStep++;
                }
                break;

            case 3:
              
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
                highlightBox.transform.position = buttonWave.transform.position;
                break;

            case 1:
                tutorialText.text = "These are the enemies. Stop them!";
                highlightBox.transform.position = enemyExample.position;
                break;

            case 2:
                tutorialText.text = "Place a tower here to defend!";
                highlightBox.transform.position = towerPoint.position;
                break;

            case 3:
                tutorialText.text = "This is your economy. Use it to place and upgrade towers.";
                highlightBox.transform.position = moneyText.position;
                break;

            case 4:
                tutorialText.text = "This is your life. If it reaches 0, you lose!";
                highlightBox.transform.position = lifeText.position;
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

    public void ResumeGame()
    {
        HideTutorial();
    }

    public void OnTowerPlaced()
    {
        if (currentStep == 2)
        {
            towerPlaced = true;
        }
    }

    public void OnCoinCollected()
    {
        if (currentStep == 3 && !coinCollected)
        {
            coinCollected = true;
            ShowStep(3);
        }
    }

    public bool IsWaitingForCoinStep()
    {
        return currentStep == 3;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void OnClickNextWave()
    {
        hasClickedNextWave = true;
    }

    IEnumerator DelayEnemiesAppearing(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        ShowStep(1);
        StartCoroutine(DelayShowTowerStep(2f));
    }

    IEnumerator DelayShowTowerStep(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        ShowStep(2);
    }
}
