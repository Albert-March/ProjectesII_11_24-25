using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public BoxCollider2D parentCollider;
    public Camera cam;
    public CinemachineVirtualCamera vcam;

    private float maxDistance = 30f;
    private float minDistance = 10f;

    private Vector3 origin;
    private Vector3 difference;
    private bool drag = false;

    private bool startAnimation = true;

    private void Start()
    {
        vcam.m_Lens.OrthographicSize = 10;
        transform.position = new Vector3(0,20,0);
        StartCoroutine(StartAnimation());
    }

    private void Update()
    {
        if (!startAnimation && !IsTutorialPaused())
        {
            HandleZoom();
            HandleDrag();
        }
        LimitCameraPosition();
    }

    private bool IsTutorialPaused()
    {
        return TutorialManager.instance != null && TutorialManager.instance.IsPaused();
    }

    private void HandleZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && vcam.m_Lens.OrthographicSize < maxDistance)
        {
            vcam.m_Lens.OrthographicSize++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && vcam.m_Lens.OrthographicSize > minDistance)
        {
            vcam.m_Lens.OrthographicSize--;
        }
    }

    private void HandleDrag()
    {
        if (Input.GetButton("Fire2"))
        {
            difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - transform.position;
            if (!drag)
            {
                drag = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        { 
            drag = false;
        }

        if (drag)
        {
            transform.position = origin - difference;
        }
    }

    private void LimitCameraPosition()
    {
        Vector2 clampedPosition = CalculateLimits();
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);
    }
    private Vector2 CalculateLimits()
    {
        float halfHeight = vcam.m_Lens.OrthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Calculate the camera's half-size based on the orthographic size and aspect ratio
        Vector2 cameraSize = new Vector2(halfWidth, halfHeight);

        // Collider's world size and position adjusted for scale
        Vector2 colliderSize = parentCollider.size * parentCollider.transform.lossyScale;
        Vector2 colliderCenter = (Vector2)parentCollider.transform.position + parentCollider.offset;

        // Calculate the boundaries, clamping them to ensure we don't exceed the collider's edges
        float minX = Mathf.Max(colliderCenter.x - (colliderSize.x / 2) + cameraSize.x, colliderCenter.x - colliderSize.x / 2);
        float maxX = Mathf.Min(colliderCenter.x + (colliderSize.x / 2) - cameraSize.x, colliderCenter.x + colliderSize.x / 2);
        float minY = Mathf.Max(colliderCenter.y - (colliderSize.y / 2) + cameraSize.y, colliderCenter.y - colliderSize.y / 2);
        float maxY = Mathf.Min(colliderCenter.y + (colliderSize.y / 2) - cameraSize.y, colliderCenter.y + colliderSize.y / 2);


        return new Vector2(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY)
        );
    }

    IEnumerator StartAnimation()
    {
        while (vcam.m_Lens.OrthographicSize < maxDistance) 
        {
            vcam.m_Lens.OrthographicSize += 0.1f;
            yield return null;
        }
        startAnimation = false;
    }
}

