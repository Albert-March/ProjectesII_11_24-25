using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{
    public int id;
	public float damage;
	public float fireRate;
	public float DPS;
	public int projectileHp;
	public float projectileSpeed;
	public float hability; //No es un float (falta definir)
	public float range;
	private SpriteRenderer sprite;

    public List<Enemy> enemiesInRange = new List<Enemy>();

    public AttackManager attackManager;

    public float lastShootTime;

    private bool canShoot;

    public void SetTowerData(TowerStats stats)
	{
        this.id = stats.id;
		this.damage = stats.damage;
		this.fireRate = stats.fireRate;
		this.DPS = stats.DPS;
		this.projectileHp = stats.projectileHp;
		this.projectileSpeed = stats.projectileSpeed;
		this.hability = stats.hability;
        this.range = stats.range;
        GameObject towerObject = Instantiate(stats.AnimationPrefab, transform.position, Quaternion.identity);
        towerObject.transform.SetParent(transform, true);
        towerObject.transform.rotation = transform.rotation;
        attackManager.SetAttackType(id);
    }

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
	}

    private void Update()
    {
        GetComponent<CircleCollider2D>().radius = range;
        if (Time.time >= lastShootTime + fireRate)
        {
            if (enemiesInRange.Count > 0)
            {
                Enemy enemyHolder = enemiesInRange[0];
                attackManager.attackType.Attack(enemyHolder);
                lastShootTime = Time.time;
            }     
        }
        
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
