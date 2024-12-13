using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverText; // Texto que aparecer� al pasar el rat�n

    private void Start()
    {
        if (hoverText != null)
        {
            hoverText.SetActive(false); // Asegurarse de que el texto est� oculto inicialmente
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(true); // Mostrar el texto al pasar el rat�n
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(false); // Ocultar el texto al salir el rat�n
        }
    }
}

