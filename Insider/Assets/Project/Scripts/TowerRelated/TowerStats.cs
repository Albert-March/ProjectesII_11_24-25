using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "ScriptableObjects/TowerStats", order = 1)]
public class TowerStats : ScriptableObject
{
	//Stats
    public int id;
    public float damage;
	public float fireRate;
	public int projectileHp;
	public float projectileSpeed;
	public float range;
	public int targetAmount;

	//Prices
	public int priceLevel_1;
	public int priceLevel_2;
	public int priceLevel_3;

	public float hability; //No es un float (falta definir)

	public float rangeOffstY;
	public GameObject AnimationPrefab;
	public GameObject prefab;
	public AttackManager attackManager;
}
