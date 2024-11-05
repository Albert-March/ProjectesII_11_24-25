using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "ScriptableObjects/TowerStats", order = 1)]
public class TowerStats : ScriptableObject
{
	public float HP;
	public float Dany;
	public float RatiDeTrets;
	public float DisparsXsegon;
	public float VidaDeProjectils;
	public float Habilitat;
	public float Rang;
	public float Detecció;
	public float Target;
	public Color colorTower;
	public GameObject prefab;
}
