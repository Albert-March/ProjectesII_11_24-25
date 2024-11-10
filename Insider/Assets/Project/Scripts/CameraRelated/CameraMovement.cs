using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public BoxCollider2D parentCollider;
	public Camera cam;

	public CinemachineVirtualCamera vcam;
	float maxDistance;
	float minDistance;

	private Vector3 Origin;
	private Vector3 Difference;

	private bool drag = false;

	private void Start()
	{
		maxDistance = 30;
		minDistance = 20;
	}
	private void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0f && vcam.m_Lens.OrthographicSize < maxDistance)
		{
			vcam.m_Lens.OrthographicSize++;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0f && vcam.m_Lens.OrthographicSize > minDistance)
		{
			vcam.m_Lens.OrthographicSize--;	
		}


		if (Input.GetButton("Fire2"))
		{
			Difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - transform.position;
			if (drag == false)
			{
				drag = true;
				Origin = cam.ScreenToWorldPoint(Input.mousePosition);
			}

		}
		else
		{
			drag = false;
		}

		if (drag)
		{
			transform.position = Origin - Difference;
		}

		transform.position = CalculateLimits();
	}

	private Vector2 CalculateLimits()
	{
		float halfHeight = cam.orthographicSize * 2;
		float halfWidth = halfHeight * cam.aspect;

		Vector2 cameraSize = new Vector2(halfWidth, halfHeight);

		float targetSpaceX = parentCollider.size.x - halfWidth;
		float targetSpaceY = parentCollider.size.y - halfHeight;

		return new Vector2(Mathf.Clamp(transform.position.x, -targetSpaceX, targetSpaceX), Mathf.Clamp(transform.position.y, -targetSpaceY, targetSpaceY));
	}
}
