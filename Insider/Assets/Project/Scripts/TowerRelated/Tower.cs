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
    GameObject towerObject;
    Animator animatorTower;
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

        GetComponent<CircleCollider2D>().radius = range;
        GetComponent<CircleCollider2D>().offset = new Vector2(0,stats.rangeOffstY);
        GameObject towerObject = Instantiate(stats.AnimationPrefab, transform.position, Quaternion.identity);
        animatorTower = towerObject.GetComponent<Animator>();
        towerObject.transform.SetParent(transform, true);
        towerObject.transform.rotation = transform.rotation;
        attackManager.SetAttackType(id);
    }

	private void Awake()
	{

	}

    private void Update()
    {
        if (animatorTower.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) { return; }
        if (enemiesInRange.Count > 0)
        {
            if (Time.time >= lastShootTime + fireRate)
            {
                animatorTower.SetBool("IsAttacking", true);
                Enemy enemyHolder = enemiesInRange[0];
                attackManager.attackType.Attack(enemyHolder);
                lastShootTime = Time.time;

            }
            else 
            {
                animatorTower.SetBool("IsAttacking", false);
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
