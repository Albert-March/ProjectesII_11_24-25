using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Tower : MonoBehaviour
{
	public float damage;
	public float fireRate;
	public float DPS;
	public float projectileHp;
	public float projectileSpeed;
	public float hability; //No es un float (falta definir)
	public float range;
	private SpriteRenderer sprite;

	public void SetTowerData(TowerStats stats)
	{
		this.damage = stats.damage;
		this.fireRate = stats.fireRate;
		this.DPS = stats.DPS;
		this.projectileHp = stats.projectileHp;
		this.projectileSpeed = stats.projectileSpeed;
		this.hability = stats.hability;
		this.range = stats.range;
		this.sprite.color = stats.colorTower;
	}

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
	}
}
