using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionManager : MonoBehaviour
{
    public GameObject dinamicPanel;
    public GameObject targetOptionsPanel;
    public Text selectedTargetText;
    private string currentTarget = "First";

    void Start()
    {
        targetOptionsPanel.SetActive(false);
    }

    
    public void ToggleTargetOptions()
    {
        targetOptionsPanel.SetActive(!targetOptionsPanel.activeSelf);
    }

    
    public void SelectTarget(string target)
    {
        int t;
        switch (target) 
        {
            case "First":
                t = 0;
                break;
            case "Last":
                t = 1;
                break;
            case "Strong":
                t = 2;
                break;
            case "Far":
                t = 3;
                break;
            default:
                t = 0;
                break;
        }
        dinamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.gameObject.transform.GetChild(2).GetComponent<Tower>().targetType = t;
        currentTarget = target;
        selectedTargetText.text = $"Target: {currentTarget}";
        targetOptionsPanel.SetActive(false);
    }
}

