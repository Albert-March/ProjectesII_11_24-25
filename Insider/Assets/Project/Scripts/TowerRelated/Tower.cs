using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Tower : MonoBehaviour
{
	public float HP;
	public float Dany;
	public float RatiDeTrets;
	public float DisparsXsegon;
	public float VidaDeProjectils;
	public float Habilitat;
	public float Rang;
	public float Detecci�;
	public float Target;
	private SpriteRenderer sprite;

	public void SetTowerData(TowerStats stats)
	{
		this.HP = stats.HP;
		this.Dany = stats.Dany;
		this.RatiDeTrets = stats.RatiDeTrets;
		this.DisparsXsegon = stats.DisparsXsegon;
		this.VidaDeProjectils = stats.VidaDeProjectils;
		this.Habilitat = stats.Habilitat;
		this.Rang = stats.Rang;
		this.Detecci� = stats.Detecci�;
		this.Target = stats.Target;
		this.sprite.color = stats.colorTower;
	}

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
	}
}
