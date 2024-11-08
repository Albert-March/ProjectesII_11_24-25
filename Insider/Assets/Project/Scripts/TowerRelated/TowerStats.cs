using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "ScriptableObjects/TowerStats", order = 1)]
public class TowerStats : ScriptableObject
{
	public float damage;
	public float fireRate;
	public float DPS;
	public float projectileHp;
	public float projectileSpeed;
	public float range;
    public float bulletSize;
    public AbilityManager ability;
    public Color colorTower;
    public GameObject prefab;
}
