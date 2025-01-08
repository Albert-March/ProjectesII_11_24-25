using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour
{
    public string levelName;          // Nombre del nivel (ej. "Corazón")
    public Text levelText;            // Texto que muestra el nombre del nivel
    private Vector3 originalScale;    // Escala original del órgano

    public AudioClip hoverSound;      // Clip de sonido que se reproducirá
    private AudioSource audioSource;  // Componente AudioSource para reproducir sonido

    void Start()
    {
        // Guardar el tamaño original
        originalScale = transform.localScale;

        // Asegurarse de que el texto esté vacío al inicio
        if (levelText != null)
        {
            levelText.text = "";
        }

        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseEnter()
    {
        // Agrandar el órgano al pasar el ratón
        transform.localScale = originalScale * 1.2f;

        // Mostrar el nombre del nivel
        if (levelText != null)
        {
            levelText.text = levelName;
        }

        // Reproducir el sonido de hover
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    void OnMouseExit()
    {
        // Restaurar el tamaño original
        transform.localScale = originalScale;

        // Ocultar el texto
        if (levelText != null)
        {
            levelText.text = "";
        }
    }
}


