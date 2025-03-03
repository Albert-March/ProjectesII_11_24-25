using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVisibilityController : MonoBehaviour
{
    public GameObject panel;
    public DinamicPanelAutocloser lockbutton;

    public void TogglePanel()
    {
        if (panel.GetComponent<Animator>().GetBool("Open"))
        {
            panel.GetComponent<Animator>().SetBool("Open", false);
            panel.GetComponent<Animator>().SetBool("Open", true);
        }
        else 
        {
            panel.GetComponent<Animator>().SetBool("Open", !panel.GetComponent<Animator>().GetBool("Open"));
        }
    }
}

