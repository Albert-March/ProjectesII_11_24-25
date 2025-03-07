using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
	//Fase 1 (Un poco hacia abajo)
	public float initialDownwardForce = 10f;
	public float downwardDamping = 2f;
	public float phase1Time = 0.2f;
	private Vector3 downwardVelocity;

	//Fase 2 (Hacia el destino)
	public float acceleration = 30f;
	public float maxSpeed = 30f;
	public Vector3 targetPosition;
	[SerializeField]private float currentSpeed = 0f;

	private bool isPhase2Started = false;
	private float elapsedTime = 0f;

	public void Initialize(Vector3 target)
	{
		targetPosition = target;
		downwardVelocity = Vector3.down * initialDownwardForce;
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
		transform.position += downwardVelocity * Time.deltaTime;
		downwardVelocity = Vector3.Lerp(downwardVelocity, Vector3.zero, downwardDamping * Time.deltaTime);
	}

	private void StartPhase2()
	{
		isPhase2Started = true;
		downwardVelocity = Vector3.zero;
	}

	private void MoveTowardsTarget()
	{
		Vector3 direction = (targetPosition - transform.position).normalized;
		if (currentSpeed < maxSpeed) 
		{
            currentSpeed += acceleration * Time.deltaTime;
        }

		transform.position += direction * currentSpeed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D col)
    {

            if (col.transform.tag == "Player")
            {
				Destroy(gameObject);
			}
    }
}
