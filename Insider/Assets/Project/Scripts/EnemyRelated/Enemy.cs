using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamage
{
    public string enemyName;
    public float movSpeed;
    public float attackSpeed;
    public float health;
    public float dmg;
    public int economyGiven;

    SpriteRenderer sprite;
    public List<Target> path;

    public EnemyManager enemyManager;

	EconomyManager economyScript;

	public int currentTarget = 0;

    private List<EnemyBehaviour> behaviours = new List<EnemyBehaviour>();

    private IDamage _damageReciver;

    private float timeSinceLastAtack = 0;

	[SerializeField] private ParticleSystem destroyParticles;
	private ParticleSystem destroyParticlesInstance;

    [SerializeField] HealthBar healthBar;

    [SerializeField] private GameObject reward;
    private GameObject rewardInstance;

	public void SetEnemyData(EnemyStats enemy)
    {
        this.enemyName = enemy.enemyName;
        this.movSpeed = enemy.movSpeed;
        this.attackSpeed = enemy.attackSpeed;
        this.health = enemy.health;
        this.dmg = enemy.dmg;
        this.economyGiven = enemy.economyGiven;
        this.sprite.color = enemy.color;
        behaviours.Add(gameObject.AddComponent<BaseMovement>());
        behaviours.Add(gameObject.AddComponent<ObjectAvoidance>());
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public float maxHealth;
    public void Start()
    {
        maxHealth = health;
	}

    public void Update()
    {
        if (_damageReciver == null) 
        {
            PlayAllBehaviours();
            return;
        }


        if (Time.time >= timeSinceLastAtack + attackSpeed)
        {
            _damageReciver.Damage(dmg);
            timeSinceLastAtack = Time.time;
        }
	}

    private void PlayAllBehaviours() 
    {
        foreach (var behaviour in behaviours)
        {
            behaviour.Behave(this, path[currentTarget]);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(currentTarget < path.Count-1){
            if(col.transform == path[currentTarget].obj.transform)
            {
                currentTarget++;
            }
        }
        else
        {
            if (col.transform == path[currentTarget].obj.transform)
            {
                _damageReciver = col.GetComponent<IDamage>();
            }
        }
    }

    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0) 
        {
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
			economyScript = FindObjectOfType<EconomyManager>();
			economyScript.economy += economyGiven;
            SpawnParticles();
            SpawnReward();

		}
		healthBar.UpdateHealthBar(health, maxHealth);
	}

	public void SpawnParticles()
	{
		destroyParticlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
	}

	public void SpawnReward()
	{
		rewardInstance = Instantiate(reward, transform.position, Quaternion.identity);
		RewardManager rewardManager = rewardInstance.GetComponent<RewardManager>();
		rewardManager = rewardInstance.AddComponent<RewardManager>();
		rewardManager.Initialize(new Vector3(0, 20, 0));
	}
}
