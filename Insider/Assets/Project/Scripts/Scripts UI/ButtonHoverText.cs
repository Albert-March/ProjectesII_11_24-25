using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverText; // Texto que aparecerá al pasar el ratón

    private void Start()
    {
        if (hoverText != null)
        {
            hoverText.SetActive(false); // Asegurarse de que el texto esté oculto inicialmente
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(true); // Mostrar el texto al pasar el ratón
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(false); // Ocultar el texto al salir el ratón
        }
    }
}

