using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionManager : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;
    public GameObject target5;

	public GameObject textDesactivatTarget1;
	public GameObject textDesactivatTarget2;
	public GameObject textDesactivatTarget3;
	public GameObject textDesactivatTarget4;
	public GameObject textDesactivatTarget5;

	private Animator dinamicPanelAnimator;

	public GameObject dinamicPanel;
    public GameObject targetOptionsPanel;
    public AudioManager audioManager;
    private string currentTarget = "First";
    private string LastTarget = "First";
    int targetType = 0;

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	void Start()
    {
		if (dinamicPanel != null)
		{
			dinamicPanelAnimator = dinamicPanel.GetComponent<Animator>();
		}
		targetOptionsPanel.SetActive(false);

		if (target1 != null)
		{
			target1.transform.position += new Vector3(3f, 0f, 0f);
			textDesactivatTarget1.SetActive(false);
		}
	}

	private void Update()
	{
		if (dinamicPanelAnimator != null && !dinamicPanelAnimator.GetBool("Open"))
		{
			if (targetOptionsPanel.activeSelf)
			{
				targetOptionsPanel.SetActive(false);
			}
		}
		else if(dinamicPanelAnimator != null && dinamicPanelAnimator.GetBool("Open") && this.gameObject.activeSelf)
		{
			if (!targetOptionsPanel.activeSelf)
			{
				targetOptionsPanel.SetActive(true);
			}
		}

		if (dinamicPanel.GetComponent<SetTowerBaseInput>().spawnTower == true)
        {
			targetType = dinamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.gameObject.transform.GetChild(2).GetComponent<Tower>().targetType;

			//switch (targetType)
			//{
			//	case 0:
			//		currentTarget = "First";
			//		break;
			//	case 1:
			//		currentTarget = "Last";
			//		break;
			//	case 2:
			//		currentTarget = "Strong";
			//		break;
			//	case 3:
			//		currentTarget = "Weak";
			//		break;
   //             case 4:
   //                 currentTarget = "Far";
			//		break;
   //             default:
			//		currentTarget = "First";
			//		break;
			//}
			//if (newTarget != currentTarget)
			//{
			//	MoveTarget(LastTarget, -0.5f);

			//	LastTarget = currentTarget;
			//	currentTarget = newTarget;

			//	MoveTarget(currentTarget, 0.5f);
			//}
		}  
	}

	public void ToggleTargetOptions()
    {
        audioManager.PlaySFX(2, 0.1f);
        //targetOptionsPanel.SetActive(!targetOptionsPanel.activeSelf);
    }

    
    public void SelectTarget(string target)
    {
        audioManager.PlaySFX(3, 0.1f);

		int t;
        switch (target) 
        {
            case "First":
                t = 0;
				currentTarget = "First";
				textDesactivatTarget1.SetActive(false);
				break;
            case "Last":
                t = 1;
				currentTarget = "Last";
				break;
            case "Strong":
                t = 2;
				currentTarget = "Strong";
				break;
            case "Weak":
                t = 3;
				currentTarget = "Weak";
				break;
            case "Far":
                t = 4;
				currentTarget = "Far";
				break;
            default:
                t = 0;
				currentTarget = "First";
				break;
        }
		if(LastTarget != target) 
		{ 
			MoveTarget(LastTarget, -3f, false);
			LastTarget = target;
			MoveTarget(currentTarget, 3f, true);
		}
		

		dinamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.gameObject.transform.GetChild(2).GetComponent<Tower>().targetType = t;
        //targetOptionsPanel.SetActive(false);
    }
	private void MoveTarget(string targetName, float offsetX, bool desactiva)
	{
		GameObject targetObject = null;
		GameObject textDesactivat = null;

		switch (targetName)
		{
			case "First":
				targetObject = target1;
				textDesactivat = textDesactivatTarget1;
				break;
			case "Last":
				targetObject = target2;
				textDesactivat = textDesactivatTarget2;
				break;
			case "Strong":
				targetObject = target3;
				textDesactivat = textDesactivatTarget3;
				break;
			case "Weak":
				targetObject = target4;
				textDesactivat = textDesactivatTarget4;
				break;
			case "Far":
				targetObject = target5;
				textDesactivat = textDesactivatTarget5;
				break;
		}

		if (targetObject != null)
		{
			targetObject.transform.position += new Vector3(offsetX, 0f, 0f);
		}

		if (desactiva)
		{
			textDesactivat.SetActive(false);
		}
		else
		{
			textDesactivat.SetActive(true);
		}
	}
}

