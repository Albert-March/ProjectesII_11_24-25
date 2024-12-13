using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicCam : MonoBehaviour
{
    public Camera OriginalCam;
    void Update()
    {
        GetComponent<Camera>().orthographicSize = OriginalCam.orthographicSize;
    }
}
