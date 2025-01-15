using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicTowerSetting : MonoBehaviour
{
    public SetTowerBaseInput dynamicPanel;

    public int towerId;
    public int targetType;

    public bool spawnTower = false;
    public bool levelUp2 = false;
    public bool levelUp3 = false;

    public void Clicked()
    {
        dynamicPanel.clickedButton = transform.parent.gameObject;
    }
}
