using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "ScriptableObjects/TowerStats", order = 1)]
public class TowerStats : ScriptableObject
{
	//Stats
    public int id;
    public int type; // 0 Base, 1 Type1, 2 Type2;
    public float damage;
	public float fireRate;
	public int projectileHp;
	public float projectileSpeed;
	public float range;
	public int targetAmount;

	//Prices
	public int priceLevel_1_Type0;
	public int priceLevel_2_Type1;
	public int priceLevel_2_Type2;
	public int priceLevel_3_Type1;
	public int priceLevel_3_Type2;

	public float rangeOffstY;
	public GameObject AnimationPrefab;
	public GameObject prefab;
	public AttackManager attackManager;
}
