using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DinamicTowerSetting : MonoBehaviour
{
    public SetTowerBaseInput dynamicPanel;

    public int towerId;
    public int targetType;

    public bool spawnTower = false;
    public bool levelUp2 = false;
    public bool levelUp3 = false;

    AudioManager audioManager;

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}
	public void Clicked()
    {
        dynamicPanel.clickedButton = transform.parent.gameObject;
		audioManager.PlaySFX(audioManager.selectTower);
	}
}
