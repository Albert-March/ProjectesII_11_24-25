using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ControlDesplegable : MonoBehaviour, IPointerExitHandler
{
    public GameObject contenedorIzquierda;   // Contenedor para los botones de la izquierda
    public GameObject contenedorDerecha;     // Contenedor para los botones de la derecha
    public GameObject[] botonesIzquierda;    // Array para los botones de la izquierda
    public GameObject[] botonesDerecha;      // Array para los botones de la derecha
    public GameObject recuadroFondo;         // Recuadro de fondo que se despliega junto con el botón principal

	public Vector2 posicionIzquierda;
    public Vector2 posicionDerecha;
    public Vector2 posicionCentral;

    private bool botonesVisibles = false;
    public float appearDuration = 0.5f;
    public float disappearDuration = 0.2f;
    public float moveToCenterDuration = 0.5f;

    private bool izquierdaBloqueada = false;
    private bool derechaBloqueada = false;

    private Vector3 nextPos;
    private Vector3 nextSize;

    private bool towerInstantiated = false;
	public void Start()
	{
		nextPos = transform.localPosition + new Vector3(0, 50, 0);
        nextSize = GetComponent<RectTransform>().sizeDelta + new Vector2(0,20);

	}
	public void Update()
	{
        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (transform.localPosition != nextPos)
        {
            if (towerInstantiated)
            {
                GetComponent<RectTransform>().sizeDelta = Vector3.MoveTowards(GetComponent<RectTransform>().sizeDelta, nextSize, 2);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPos, 2);
            }
        }
	}
   
    public void OnPointerExit(PointerEventData eventData)
    {
        if(botonesVisibles)
            AlternarBotones();
    }
    public void AlternarBotones()
    {
        botonesVisibles = !botonesVisibles;

        // Mostrar u ocultar el recuadro de fondo
        recuadroFondo.SetActive(botonesVisibles);

        if (botonesVisibles)
        {
            // Si la columna izquierda está bloqueada, centra la columna derecha
            if (izquierdaBloqueada)
            {
                StartCoroutine(AnimateMoveToCenter(contenedorDerecha, posicionCentral));
                StartCoroutine(AnimateInGroup(botonesDerecha, posicionCentral));
            }
            // Si la columna derecha está bloqueada, centra la columna izquierda
            else if (derechaBloqueada)
            {
                StartCoroutine(AnimateMoveToCenter(contenedorIzquierda, posicionCentral));
                StartCoroutine(AnimateInGroup(botonesIzquierda, posicionCentral));
            }
            else
            {
                // Si ambas columnas están visibles, muestra cada una en su posición original
                SetContainerPosition(contenedorIzquierda, posicionIzquierda);
                SetContainerPosition(contenedorDerecha, posicionDerecha);
                StartCoroutine(AnimateInGroup(botonesIzquierda, posicionIzquierda));
                StartCoroutine(AnimateInGroup(botonesDerecha, posicionDerecha));
            }
        }
        else
        {
            // Oculta ambas columnas en su posición original si están visibles y no bloqueadas
            if (!izquierdaBloqueada)
            {
                StartCoroutine(AnimateOutGroup(botonesIzquierda));
            }

            if (!derechaBloqueada)
            {
                StartCoroutine(AnimateOutGroup(botonesDerecha));
            }
        }
    }

    // Método para ocultar los botones de la derecha y centrar los de la izquierda
    public void OcultarBotonesDerecha()
    {
        if (!derechaBloqueada)
        {
            StartCoroutine(AnimateOutGroup(botonesDerecha));
            derechaBloqueada = true; // Bloquea los botones de la derecha
            StartCoroutine(AnimateMoveToCenter(contenedorIzquierda, posicionCentral)); // Mueve la columna izquierda al centro con animación
            towerInstantiated = true;
        }
    }

    // Método para ocultar los botones de la izquierda y centrar los de la derecha
    public void OcultarBotonesIzquierda()
    {
        if (!izquierdaBloqueada)
        {
            StartCoroutine(AnimateOutGroup(botonesIzquierda));
            izquierdaBloqueada = true; // Bloquea los botones de la izquierda
            StartCoroutine(AnimateMoveToCenter(contenedorDerecha, posicionCentral)); // Mueve la columna derecha al centro con animación
            towerInstantiated = true;
        }
    }

    // Corutina para animar la aparición de los botones (solo escalado) en una posición inicial específica
    private IEnumerator AnimateInGroup(GameObject[] botones, Vector2 posicionInicial)
    {
        foreach (GameObject boton in botones)
        {
            RectTransform rt = boton.GetComponent<RectTransform>();
            rt.localScale = Vector3.zero;
            boton.SetActive(true);

            if (rt != null)
            {
                rt.anchoredPosition = posicionInicial; // Establece la posición inicial

                for (float t = 0; t <= appearDuration; t += Time.deltaTime)
                {
                    float normalizedTime = t / appearDuration;
                    rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);  // Gradualmente aumenta la escala
                    yield return null;
                }
                rt.localScale = Vector3.one;
            }
        }
    }

    // Corutina para animar la desaparición de los botones (solo escalado)
    private IEnumerator AnimateOutGroup(GameObject[] botones)
    {
        foreach (GameObject boton in botones)
        {
            RectTransform rt = boton.GetComponent<RectTransform>();

            if (rt != null)
            {
                for (float t = 0; t <= disappearDuration; t += Time.deltaTime)
                {
                    float normalizedTime = t / disappearDuration;
                    rt.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);  // Gradualmente disminuye la escala
                    yield return null;
                }
                rt.localScale = Vector3.zero;
                boton.SetActive(false);
            }
        }
    }

    // Corutina para animar el movimiento hacia el centro de la columna
    private IEnumerator AnimateMoveToCenter(GameObject contenedor, Vector2 posicionFinal)
    {
        RectTransform rt = contenedor.GetComponent<RectTransform>();

        if (rt != null)
        {
            Vector2 posicionInicial = rt.anchoredPosition;

            for (float t = 0; t <= moveToCenterDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / moveToCenterDuration;
                rt.anchoredPosition = Vector2.Lerp(posicionInicial, posicionFinal, normalizedTime); // Interpola la posición
                yield return null;
            }

            rt.anchoredPosition = posicionFinal; // Asegura que termine exactamente en el centro
        }
    }

    // Método para establecer la posición de un contenedor sin animación
    private void SetContainerPosition(GameObject contenedor, Vector2 nuevaPosicion)
    {
        RectTransform rt = contenedor.GetComponent<RectTransform>();

        if (rt != null)
        {
            rt.anchoredPosition = nuevaPosicion;
        }
    }


}









