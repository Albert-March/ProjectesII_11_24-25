using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomAnimation : MonoBehaviour
{
    public float startSize = 10f;      // Tamaño inicial (para cámaras ortográficas)
    public float targetSize = 5f;     // Tamaño final después del zoom
    public float zoomDuration = 1f;   // Duración del zoom en segundos

    private Camera cam;
    private float elapsedTime;

    void Start()
    {
        cam = Camera.main; // Obtén la cámara principal
        if (cam.orthographic)
        {
            cam.orthographicSize = startSize; // Configura el tamaño inicial
        }
        else
        {
            cam.fieldOfView = startSize; // Configura el FOV inicial para cámaras de perspectiva
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
                // Interpolación para cámaras ortográficas
                cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            }
            else
            {
                // Interpolación para cámaras de perspectiva
                cam.fieldOfView = Mathf.Lerp(startSize, targetSize, t);
            }
        }
    }
}

