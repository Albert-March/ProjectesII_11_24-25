using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class FollowDeformingMesh : MonoBehaviour
{
    public Transform movingSkin; // The SpriteRenderer for the background
    public int vertexIndex = 0;           // The vertex index to follow

    private SpriteSkin spriteSkin;

    void Start()
    {
        // Get the SpriteSkin component
        spriteSkin = movingSkin.GetComponent<SpriteSkin>();

        if (spriteSkin == null)
        {
            Debug.LogError("SpriteSkin component is missing!");
            return;
        }

        // Validate the SpriteSkin by checking required components and properties
        if (!spriteSkin.enabled || spriteSkin.boneTransforms == null || spriteSkin.boneTransforms.Length == 0)
        {
            Debug.LogError("SpriteSkin is not properly set up. Ensure bones are assigned and SpriteSkin is enabled.");
            spriteSkin = null; // Disable further processing
            return;
        }
    }

    void LateUpdate()
    {
        if (spriteSkin == null)
            return;

        // Get the deformed vertices as an IEnumerable
        var deformedVertices = spriteSkin.GetDeformedVertexPositionData();

        // Convert to an array for indexed access
        Vector3[] verticesArray = deformedVertices.ToArray();

        if (vertexIndex < 0 || vertexIndex >= verticesArray.Length)
        {
            Debug.LogError("Vertex index is out of bounds!");
            return;
        }

        // Transform the deformed vertex position to world space
        Vector3 localPosition = verticesArray[vertexIndex];
        Vector3 worldPosition = movingSkin.TransformPoint(localPosition);

        // Update the position of the element
        transform.position = worldPosition;
    }
}
