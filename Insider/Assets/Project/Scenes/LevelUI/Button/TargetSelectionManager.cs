using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionManager : MonoBehaviour
{
    public GameObject dinamicPanel;
    public GameObject targetOptionsPanel;
    public AudioManager audioManager;
    public Text selectedTargetText;
    private string currentTarget = "First";
    int targetType = 0;

    void Start()
    {
        targetOptionsPanel.SetActive(false);
    }

	private void Update()
	{
		if (dinamicPanel.GetComponent<SetTowerBaseInput>().spawnTower == true)
        {
			targetType = dinamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.gameObject.transform.GetChild(2).GetComponent<Tower>().targetType;
			switch (targetType)
			{
				case 0:
					currentTarget = "First";
					break;
				case 1:
					currentTarget = "Last";
					break;
				case 2:
					currentTarget = "Strong";
					break;
				case 3:
					currentTarget = "Weak";
					break;
                case 4:
                    currentTarget = "Far";
                    break;
                default:
					currentTarget = "First";
					break;
			}
			selectedTargetText.text = $" ↓ Target: {currentTarget}";
		}  
	}

	public void ToggleTargetOptions()
    {
        audioManager.PlaySFX(2, 0.1f);
        targetOptionsPanel.SetActive(!targetOptionsPanel.activeSelf);
    }

    
    public void SelectTarget(string target)
    {
        audioManager.PlaySFX(3, 0.1f);

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
            case "Weak":
                t = 3;
                break;
            case "Far":
                t = 4;
                break;
            default:
                t = 0;
                break;
        }
		dinamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.gameObject.transform.GetChild(2).GetComponent<Tower>().targetType = t;
        targetOptionsPanel.SetActive(false);
    }
}

