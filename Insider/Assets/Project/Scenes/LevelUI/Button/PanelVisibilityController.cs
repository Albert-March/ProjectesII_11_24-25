using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelVisibilityController : MonoBehaviour
{
    public GameObject panel;
    public DinamicPanelAutocloser lockbutton;

	private void Update()
	{
		if (panel.GetComponent<Animator>().GetBool("Open") == true && Input.GetMouseButtonDown(0))
		{
			panel.GetComponent<Animator>().SetBool("Open", false);
		}
	}
	public void TogglePanel()
    {
        if (panel.GetComponent<Animator>().GetBool("Open"))
        {
            panel.GetComponent<Animator>().SetBool("Open", false);
        }
        else 
        {
            panel.GetComponent<Animator>().SetBool("Open", !panel.GetComponent<Animator>().GetBool("Open"));
        }
    }
}

