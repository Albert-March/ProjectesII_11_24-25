using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomAnimation : MonoBehaviour
{
    public float startSize = 10f;      // Tama�o inicial (para c�maras ortogr�ficas)
    public float targetSize = 5f;     // Tama�o final despu�s del zoom
    public float zoomDuration = 1f;   // Duraci�n del zoom en segundos

    private Camera cam;
    private float elapsedTime;

    void Start()
    {
        cam = Camera.main; // Obt�n la c�mara principal
        if (cam.orthographic)
        {
            cam.orthographicSize = startSize; // Configura el tama�o inicial
        }
        else
        {
            cam.fieldOfView = startSize; // Configura el FOV inicial para c�maras de perspectiva
        }
        elapsedTime = 0f;
    }

    void Update()
    {
        if (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime; // Incrementa el tiempo transcurrido
            float t = elapsedTime / zoomDuration; // Normaliza entre 0 y 1

            if (cam.orthographic)
            {
                // Interpolaci�n para c�maras ortogr�ficas
                cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            }
            else
            {
                // Interpolaci�n para c�maras de perspectiva
                cam.fieldOfView = Mathf.Lerp(startSize, targetSize, t);
            }
        }
    }
}

