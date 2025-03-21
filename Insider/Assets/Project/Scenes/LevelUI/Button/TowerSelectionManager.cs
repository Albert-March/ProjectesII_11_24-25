using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionManager : MonoBehaviour
{
    //public GameObject[] towerButtons;
    public GameObject[] upgradeButtons1;
    public GameObject[] upgradeButtons2;
    public GameObject[] targetUI;
    public GameObject dynamicPanel;

    private int towerState = 0;

    void Start()
    {
        //SetActiveButtons(towerButtons, true);
        SetActiveButtons(upgradeButtons1, false);
        SetActiveButtons(upgradeButtons2, false);
        SetActiveButtons(targetUI, false);
    }

    private void Update()
    {
        if (towerState == 0)
        {
            //SetActiveButtons(towerButtons, true);
            SetActiveButtons(upgradeButtons1, false);
            SetActiveButtons(upgradeButtons2, false);
            SetActiveButtons(targetUI, false);

        }
        else if (towerState == 1)
        {
            //SetActiveButtons(towerButtons, false);
            SetActiveButtons(upgradeButtons1, true);
            SetActiveButtons(upgradeButtons2, false);
            SetActiveButtons(targetUI, true);
        }
        else if (towerState == 2)
        {
            //SetActiveButtons(towerButtons, false);
            SetActiveButtons(upgradeButtons2, true);
            SetActiveButtons(targetUI, true);
        }
        else if (towerState == 3)
        {
            //SetActiveButtons(towerButtons, false);
            SetActiveButtons(upgradeButtons1, false);
            SetActiveButtons(upgradeButtons2, false);
            SetActiveButtons(targetUI, true);
        }


        if (dynamicPanel.GetComponent<SetTowerBaseInput>().clickedButton != null) 
        {
            towerState = 0;
            if (dynamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().spawnTower) { towerState = 1; }
            if (dynamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp2) { towerState = 2; }
            if (dynamicPanel.GetComponent<SetTowerBaseInput>().clickedButton.transform.GetChild(0).GetComponent<DinamicTowerSetting>().levelUp3) { towerState = 3; }
        }
        else { towerState = 0; }


    }


    // Método para gestionar botones dinámicamente
    private void SetActiveButtons(GameObject[] buttons, bool isActive)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(isActive);
        }
    }
}




