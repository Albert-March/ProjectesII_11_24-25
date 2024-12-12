using System.Collections;
using UnityEngine;

public class ToggleAnimatedButtons : MonoBehaviour
{
    public GameObject[] buttons; // Los botones a animar
    public float animationDuration = 0.5f; // Duración de la animación para cada botón
    public float delayBetweenButtons = 0.1f; // Retraso entre cada botón

    private bool isActive = false; // Estado de los botones
    private Vector3[] originalScales; // Escalas originales de los botones

    private void Start()
    {
        // Guardar la escala original de cada botón
        originalScales = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            originalScales[i] = buttons[i].transform.localScale;
            buttons[i].SetActive(false); // Asegurarse de que los botones estén desactivados al inicio
        }
    }

    public void ToggleButtons()
    {
        isActive = !isActive;

        if (isActive)
        {
            StartCoroutine(ShowButtonsSequentially());
        }
        else
        {
            StartCoroutine(HideButtonsSequentially());
        }
    }

    private IEnumerator ShowButtonsSequentially()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject button = buttons[i];
            button.SetActive(true); // Activar el botón
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.localScale = Vector3.zero; // Iniciar con escala cero
            float elapsed = 0;

            // Animar la escala del botón desde cero hasta su tamaño original
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(0, 1, elapsed / animationDuration);
                rect.localScale = originalScales[i] * scale; // Escalar proporcionalmente al tamaño original
                yield return null;
            }

            rect.localScale = originalScales[i]; // Asegurarse de que la escala final sea la original
            yield return new WaitForSeconds(delayBetweenButtons); // Retraso antes de animar el siguiente botón
        }
    }

    private IEnumerator HideButtonsSequentially()
    {
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            GameObject button = buttons[i];
            RectTransform rect = button.GetComponent<RectTransform>();
            float elapsed = 0;

            // Animar la escala del botón desde su tamaño original hasta cero
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(1, 0, elapsed / animationDuration);
                rect.localScale = originalScales[i] * scale; // Escalar proporcionalmente al tamaño original
                yield return null;
            }

            button.SetActive(false); // Desactivar el botón después de la animación
            yield return new WaitForSeconds(delayBetweenButtons); // Retraso antes de animar el siguiente botón
        }
    }
}


