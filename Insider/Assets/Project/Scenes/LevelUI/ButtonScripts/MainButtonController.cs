using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonController : MonoBehaviour
{
    public GameObject dynamicPanel; // Panel que contiene los botones secundarios y upgrades

    void Start()
    {
        if (dynamicPanel != null)
        {
            dynamicPanel.SetActive(false); // Asegurarse de que el panel esté oculto al inicio
        }
    }

    void OnMouseDown()
    {
        if (dynamicPanel != null)
        {
            dynamicPanel.SetActive(!dynamicPanel.activeSelf); // Mostrar/Ocultar el panel
        }
    }
}

