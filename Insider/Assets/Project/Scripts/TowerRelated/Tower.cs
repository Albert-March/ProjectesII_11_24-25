using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tower : MonoBehaviour
{
	//Stats
    public int id;
    public int type;
	public float damage;
	public float fireRate;
	public float DPS;
	public int projectileHp;
	public float projectileSpeed;
	public float range;
	public int targetAmount;
    //Prices
    public int priceLevel_1_Type0;
    public int priceLevel_2_Type1;
    public int priceLevel_2_Type2;
    public int priceLevel_3_Type1;
    public int priceLevel_3_Type2;

	public List<Sprite> levelSprites = new List<Sprite>();
	public Image levelIndicatorImage;

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
		this.type = stats.type;
		this.damage = stats.damage;
		this.fireRate = stats.fireRate;
		this.projectileHp = stats.projectileHp;
		this.projectileSpeed = stats.projectileSpeed;
		this.range = stats.range;
		this.targetAmount = stats.targetAmount;

		this.priceLevel_1_Type0 = stats.priceLevel_1_Type0;
		this.priceLevel_2_Type1 = stats.priceLevel_2_Type1;
		this.priceLevel_2_Type2 = stats.priceLevel_2_Type2;
		this.priceLevel_3_Type1 = stats.priceLevel_3_Type1;
		this.priceLevel_3_Type2 = stats.priceLevel_3_Type2;


		GetComponent<CircleCollider2D>().radius = range;
        GetComponent<CircleCollider2D>().offset = new Vector2(0,stats.rangeOffstY);

        if (currentLevel == 1 && animatorTower == null)
        {
            GameObject towerObject = Instantiate(stats.AnimationPrefab, transform.position, Quaternion.identity);
            animatorTower = towerObject.GetComponent<Animator>();
            towerObject.transform.SetParent(transform, true);
            towerObject.transform.rotation = transform.rotation;
        }

		CreateOrUpdateLevelIndicator();

		attackManager.SetAttackType(id);
    }

	private void CreateOrUpdateLevelIndicator()
	{
		if (levelIndicatorImage == null)
		{
			// Crear Canvas
			GameObject canvasGO = new GameObject("LevelCanvas");
			canvasGO.transform.SetParent(transform);
			canvasGO.transform.localPosition = new Vector3(1f, -1f, 0);

			Canvas canvas = canvasGO.AddComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.sortingLayerName = "UI";
			canvas.sortingOrder = 100;
			canvas.renderMode = RenderMode.WorldSpace;
			canvas.scaleFactor = 10f;

			CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
			scaler.dynamicPixelsPerUnit = 10f;

			// Crear la Image
			GameObject imageGO = new GameObject("LevelImage");
			imageGO.transform.SetParent(canvasGO.transform);
			levelIndicatorImage = imageGO.AddComponent<Image>();
			levelIndicatorImage.rectTransform.sizeDelta = new Vector2(1f, 1f);
			imageGO.transform.localPosition = Vector3.zero;
		}

		// Asignar sprite
		if (levelSprites.Count >= currentLevel && levelIndicatorImage != null)
		{
			levelIndicatorImage.sprite = levelSprites[currentLevel - 1];
		}
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
        audioManager.PlaySFX(5, 0.1f);
    }

    private void Update()
    {
        if (animatorTower.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) { return; }
        if (enemiesInRange.Count > 0)
        {
            attackManager.attackType.Attack(enemiesInRange, targetAmount, animatorTower, audioManager, targetType, targetManager);
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
