using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionManager : MonoBehaviour
{
    public GameObject targetOptionsPanel;
    public Text selectedTargetText;
    private string currentTarget = "Ninguno";

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
        currentTarget = target;
        selectedTargetText.text = $"Target: {currentTarget}";
        targetOptionsPanel.SetActive(false);
    }
}

