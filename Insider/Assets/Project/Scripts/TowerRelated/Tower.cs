using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{
	//Stats
    public int id;
	public float damage;
	public float fireRate;
	public float DPS;
	public int projectileHp;
	public float projectileSpeed;
	public float range;

	//Prices
	public int priceLevel_1;
	public int priceLevel_2;
	public int priceLevel_3;

	public float hability; //No es un float (falta definir)

	private SpriteRenderer sprite;
    GameObject towerObject;
    Animator animatorTower;
    public List<Enemy> enemiesInRange = new List<Enemy>();

    public int targetType = 0;
    public AttackManager attackManager;
    public TargetingManager targetManager;

    public float lastShootTime;

    private bool canShoot;

	public int currentLevel = 1;
	public const int maxLevel = 3;

	AudioManager audioManager;
	public void SetTowerData(TowerStats stats)
	{
		this.id = stats.id;
		this.damage = stats.damage;
		this.fireRate = stats.fireRate;
		this.DPS = stats.DPS;
		this.projectileHp = stats.projectileHp;
		this.projectileSpeed = stats.projectileSpeed;
		this.range = stats.range;

		this.priceLevel_1 = stats.priceLevel_1;
		this.priceLevel_2 = stats.priceLevel_2;
		this.priceLevel_3 = stats.priceLevel_3;

		this.hability = stats.hability;

		GetComponent<CircleCollider2D>().radius = range;
        GetComponent<CircleCollider2D>().offset = new Vector2(0,stats.rangeOffstY);

        if (currentLevel == 1 && animatorTower == null)
        {
            GameObject towerObject = Instantiate(stats.AnimationPrefab, transform.position, Quaternion.identity);
            animatorTower = towerObject.GetComponent<Animator>();
            towerObject.transform.SetParent(transform, true);
            towerObject.transform.rotation = transform.rotation;
        }

        attackManager.SetAttackType(id);
    }

	public void LevelUp(TowerStats stats)
	{
		if (currentLevel < maxLevel)
		{
			currentLevel++;
			SetTowerData(stats);
		}
	}

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

    private void Update()
    {
        if (animatorTower.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) { return; }
        if (enemiesInRange.Count > 0)
        {
            if(id == 3)
            {
				animatorTower.SetBool("IsAttacking", true);
			}
			if (Time.time >= lastShootTime + fireRate)
            {
				if(id != 3)
				{
					animatorTower.SetBool("IsAttacking", true);
				}
			    Enemy enemyHolder = targetManager.GetEnemyTargetFromList(enemiesInRange, targetType);
                attackManager.attackType.Attack(enemyHolder, animatorTower, audioManager);
				audioManager.PlaySFX(1, 0.1f);
				lastShootTime = Time.time;
            }
		    else if(id != 3)
		    {
			    animatorTower.SetBool("IsAttacking", false);
		    }
		}
		else
		{
			animatorTower.SetBool("IsAttacking", false);
		}
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;

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
