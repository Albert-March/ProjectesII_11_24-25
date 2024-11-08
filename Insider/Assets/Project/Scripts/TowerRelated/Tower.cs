using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Tower : MonoBehaviour
{
    public int id;

	public float damage;
	public float fireRate;
	public float DPS;
	public float projectileHp;
	public float projectileSpeed;
	public float hability; //No es un float (falta definir)
	public float range;
	private SpriteRenderer sprite;

    public List<Enemy> enemiesInRange = new List<Enemy>();

    public AttackManager attackManager;

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
        attackManager.Set(id);
    }

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
	}
    private void Update()
    {
        attackManager.Attack(enemiesInRange[0]);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {

            if (!enemiesInRange.Contains(other.GetComponent<Enemy>()))
            {
                enemiesInRange.Add(other.GetComponent<Enemy>());
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {

            if (enemiesInRange.Contains(other.GetComponent<Enemy>()))
            {
                enemiesInRange.Remove(other.GetComponent<Enemy>());
            }

        }
    }
}
