using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
	// Fase 1 (Un poco hacia abajo)
	public float initialDownwardForce = 10f;
	public float downwardDamping = 2f;
	public float phase1Time = 0.2f;
	private Vector2 downwardVelocity;

	// Fase 2 (Hacia el destino)
	public float acceleration = 30f;
	public float maxSpeed = 30f;
	public Vector2 targetPosition;
	private float currentSpeed = 0f;

	private bool isPhase2Started = false;
	private float elapsedTime = 0f;

	private Rigidbody2D rb;

	public void Initialize(Vector2 target)
	{
		targetPosition = target;
		downwardVelocity = Vector2.down * initialDownwardForce;
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		rb.velocity = downwardVelocity;
	}

	private void Update()
	{
		elapsedTime += Time.deltaTime;

		if (elapsedTime < phase1Time)
		{
			ApplyDownwardForce();
		}
		else if (!isPhase2Started)
		{
			StartPhase2();
		}
		else
		{
			MoveTowardsTarget();
		}
	}

	private void ApplyDownwardForce()
	{
		rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, downwardDamping * Time.deltaTime);
	}

	private void StartPhase2()
	{
		isPhase2Started = true;
		rb.velocity = Vector2.zero;
	}

	private void MoveTowardsTarget()
	{
		Vector2 direction = (targetPosition - rb.position).normalized;
		if (currentSpeed < maxSpeed)
		{
			currentSpeed += acceleration * Time.deltaTime;
		}

		rb.velocity = direction * currentSpeed;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
