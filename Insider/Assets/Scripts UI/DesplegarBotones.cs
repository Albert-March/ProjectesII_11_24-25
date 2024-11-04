using UnityEngine;

public class ControlDesplegable : MonoBehaviour
{
    public GameObject[] botonesIzquierda;
    public GameObject[] botonesDerecha;   
    private bool botonesVisibles = false; 
 
    public void AlternarBotones()
    {
        botonesVisibles = !botonesVisibles;

        foreach (GameObject boton in botonesIzquierda)
        {
            boton.SetActive(botonesVisibles);
        }

        foreach (GameObject boton in botonesDerecha)
        {
            boton.SetActive(botonesVisibles);
        }
    }

    public void OcultarBotonesDerecha()
    {
        foreach (GameObject boton in botonesDerecha)
        {
            boton.SetActive(false);
        }
    }

    public void OcultarBotonesIzquierda()
    {
        foreach (GameObject boton in botonesIzquierda)
        {
            boton.SetActive(false);
        }
    }
}
