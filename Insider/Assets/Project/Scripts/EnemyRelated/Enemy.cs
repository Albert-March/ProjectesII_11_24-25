using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamage, IHealable, IMovable
{
    public int id;
    public float movSpeed;
    public float attackSpeed;
    public float health;
    public float dmg;
    public int economyGiven;
    public Vector2 visualSize;
    public CircleCollider2D EnemyCollider;
    public GameObject animationGO;

    public EnemyTypeManager enemyTypeManager;

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
    [SerializeField] GameObject hpb;

    [SerializeField] private GameObject reward;
    private GameObject rewardInstance;

    AudioManager audioManager;

	public void SetEnemyData(EnemyStats enemy)
    {
        this.id = enemy.id;
        this.movSpeed = enemy.movSpeed;
        this.attackSpeed = enemy.attackSpeed;
        this.health = enemy.health;
        this.dmg = enemy.dmg;
        this.economyGiven = enemy.economyGiven;

        //InstantiateAnimation:

        visualSize = enemy.size;
        animationGO = Instantiate(enemy.AnimationsPrefab, transform);
        animationGO.transform.localScale = visualSize;
        EnemyCollider.radius *= (visualSize.x + visualSize.y);

        //this.sprite.color = enemy.color;
        behaviours.Add(gameObject.AddComponent<BaseMovement>());
        behaviours.Add(gameObject.AddComponent<ObjectAvoidance>());
        behaviours.Add(gameObject.AddComponent<BoidMovement>());

		enemyTypeManager.SetEnemyType(id);
	}

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<HealthBar>();
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

    public float maxHealth;
	private float originalSpeed;
	private bool hasExitedRageZone = false;
	private float speedResetTime = 0f;

	public void Start()
    {
        maxHealth = health;
		originalSpeed = movSpeed;
	}

    public void Update()
    {
        if (this.health == maxHealth)
        {
            hpb.SetActive(false);
        }
        else 
        {
            hpb.SetActive(true);
        }
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

		if (hasExitedRageZone && Time.time >= speedResetTime)
		{
			movSpeed = originalSpeed;
			hasExitedRageZone = false;
            speedResetTime = 0;
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
            economyScript = FindObjectOfType<EconomyManager>();
			economyScript.economy += economyGiven;
            
            SpawnParticles();
			IRewardDropper rewardDropper = GetComponent<IRewardDropper>();
			if (rewardDropper != null)
			{
				rewardDropper.SpawnReward(path);
			}
			else
			{
				Debug.LogWarning("No se encontr� un componente que pueda generar recompensas.");
			}
			audioManager.PlaySFX(0, 0.1f);
            animationGO.GetComponent<Animator>().SetBool("Death", true);
            if (animationGO.GetComponent<Animator>())
            {
                
            }
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
            
        }
		healthBar.UpdateHealthBar(health, maxHealth);
	}

	public void SpawnParticles()
	{
		destroyParticlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
	}
	public void Heal(float amount)
	{
		health += amount;
		if (health > maxHealth)
		{
			health = maxHealth;
		}
		healthBar.UpdateHealthBar(health, maxHealth);
	}
	public void Rage(float speedMultiplier)
	{
		movSpeed = originalSpeed * speedMultiplier; // Aumenta la velocidad
	}

	public void ResetSpeed()
	{
		movSpeed = originalSpeed; // Restaura la velocidad original
	}
}
