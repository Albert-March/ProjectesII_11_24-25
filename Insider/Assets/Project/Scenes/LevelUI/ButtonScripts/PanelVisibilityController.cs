using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVisibilityController : MonoBehaviour
{
    public GameObject panel;

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}

