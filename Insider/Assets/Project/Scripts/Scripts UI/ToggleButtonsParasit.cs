using System.Collections;
using UnityEngine;

public class ToggleAnimatedButtons : MonoBehaviour
{
    public GameObject[] buttons; // Los botones a animar
    public float animationDuration = 0.5f; // Duraci�n de la animaci�n para cada bot�n
    public float delayBetweenButtons = 0.1f; // Retraso entre cada bot�n

    private bool isActive = false; // Estado de los botones
    private Vector3[] originalScales; // Escalas originales de los botones

    private void Start()
    {
        // Guardar la escala original de cada bot�n
        originalScales = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            originalScales[i] = buttons[i].transform.localScale;
            buttons[i].SetActive(false); // Asegurarse de que los botones est�n desactivados al inicio
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
            button.SetActive(true); // Activar el bot�n
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.localScale = Vector3.zero; // Iniciar con escala cero
            float elapsed = 0;

            // Animar la escala del bot�n desde cero hasta su tama�o original
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(0, 1, elapsed / animationDuration);
                rect.localScale = originalScales[i] * scale; // Escalar proporcionalmente al tama�o original
                yield return null;
            }

            rect.localScale = originalScales[i]; // Asegurarse de que la escala final sea la original
            yield return new WaitForSeconds(delayBetweenButtons); // Retraso antes de animar el siguiente bot�n
        }
    }

    private IEnumerator HideButtonsSequentially()
    {
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            GameObject button = buttons[i];
            RectTransform rect = button.GetComponent<RectTransform>();
            float elapsed = 0;

            // Animar la escala del bot�n desde su tama�o original hasta cero
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(1, 0, elapsed / animationDuration);
                rect.localScale = originalScales[i] * scale; // Escalar proporcionalmente al tama�o original
                yield return null;
            }

            button.SetActive(false); // Desactivar el bot�n despu�s de la animaci�n
            yield return new WaitForSeconds(delayBetweenButtons); // Retraso antes de animar el siguiente bot�n
        }
    }
}


