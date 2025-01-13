using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionManager : MonoBehaviour
{
    public GameObject[] towerButtons; // Botones secundarios para seleccionar torres
    public GameObject[] upgradeButtons; // Botones de mejora de la torre
    public GameObject dynamicPanel; // Panel que contiene los botones secundarios
    private bool isTowerSelected = false; // Estado que indica si ya se seleccion� una torre

    void Start()
    {
        // Asegurarnos de que los botones de mejora est�n desactivados al inicio
        SetActiveButtons(towerButtons, false);
        SetActiveButtons(upgradeButtons, false);
    }

    // M�todo llamado al presionar el bot�n principal
    public void OnMainButtonClick()
    {
        if (isTowerSelected)
        {
            // Mostrar botones de mejora
            SetActiveButtons(towerButtons, false);
            SetActiveButtons(upgradeButtons, true);
        }
        else
        {
            // Mostrar botones secundarios
            SetActiveButtons(towerButtons, true);
            SetActiveButtons(upgradeButtons, false);
        }
    }

    // M�todo para seleccionar una torre (llamado al presionar un bot�n secundario)
    public void OnTowerSelected(int towerIndex)
    {
        isTowerSelected = true; // Cambiar el estado a torre seleccionada
        SetActiveButtons(towerButtons, false); // Ocultar botones secundarios
        SetActiveButtons(upgradeButtons, true); // Mostrar botones de mejora

        Debug.Log($"Torre seleccionada: {towerIndex}");
    }

    // M�todo para gestionar botones din�micamente
    private void SetActiveButtons(GameObject[] buttons, bool isActive)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(isActive);
        }
    }

    // M�todo para seleccionar una mejora
    public void OnUpgradeSelected(int upgradeIndex)
    {
        Debug.Log($"Mejora seleccionada: {upgradeIndex}");
        // Aqu� puedes manejar l�gica adicional para aplicar la mejora
    }
}




