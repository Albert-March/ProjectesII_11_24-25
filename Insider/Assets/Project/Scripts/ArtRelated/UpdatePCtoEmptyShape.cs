using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class UpdatePCtoEmptyShape : MonoBehaviour
{
    public Transform pointsParent;
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        UpdateColliderPoints();
    }

    void Update()
    {
        // Continuously update the collider points if needed
        UpdateColliderPoints();
    }

    void UpdateColliderPoints()
    {
        if (pointsParent == null || polygonCollider == null)
            return;

        Transform[] pointTransforms = pointsParent.GetComponentsInChildren<Transform>();
        Vector2[] newPoints = new Vector2[pointTransforms.Length - 1];

        for (int i = 1; i < pointTransforms.Length; i++)
        {
            newPoints[i - 1] = transform.InverseTransformPoint(pointTransforms[i].position);
        }

        polygonCollider.points = newPoints;
    }
}
