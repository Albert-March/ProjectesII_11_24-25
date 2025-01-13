using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFollowCamera : MonoBehaviour
{
    public Camera targetCamera;
    public Vector3 offset = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (targetCamera != null)
        {
                        Vector3 cameraCenter = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, targetCamera.nearClipPlane));
            transform.position = cameraCenter + offset;
        }
    }
}

