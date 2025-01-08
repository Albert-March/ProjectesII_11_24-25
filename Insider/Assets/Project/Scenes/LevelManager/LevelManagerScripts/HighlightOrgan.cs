using UnityEngine;
using UnityEngine.UI;

public class HighlightOrgan : MonoBehaviour
{
    public Image outlineImage; // Asigna la imagen de contorno desde el Inspector.

    public void Highlight(bool enable)
    {
        outlineImage.enabled = enable; // Muestra u oculta la imagen.
    }
}


