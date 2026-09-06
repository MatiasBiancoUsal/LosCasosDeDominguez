using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReveladoFoto : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referencias UI")]
    public RectTransform zonaAgua;
    public Image imagenFoto; // La imagen de la foto
    public GameObject botonVerFoto; // El botón para inspeccionar la foto revelada
    public GameObject popUpVistaDetallada; // La pantalla completa con la foto

    [Header("Ajustes de Revelado")]
    public float duracionAnimacion = 2.0f; // Segundos que tarda en revelar
    private bool yaEstaRevelada = false;
    private bool estaEnAgua = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 posicionInicial;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // La foto arranca totalmente en negro/oscura
        if (imagenFoto != null)
        {
            imagenFoto.color = Color.black;
        }

        // Nos aseguramos de que el botón y el pop-up empiecen ocultos
        if (botonVerFoto != null) botonVerFoto.SetActive(false);
        if (popUpVistaDetallada != null) popUpVistaDetallada.SetActive(false);
    }

    // --- LÓGICA DE ARRASTRE ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        posicionInicial = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.lossyScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Verifica si se soltó sobre la cubeta de agua
        if (RectTransformUtility.RectangleContainsScreenPoint(zonaAgua, Input.mousePosition))
        {
            if (!yaEstaRevelada)
            {
                estaEnAgua = true;
                StartCoroutine(AnimacionRevelado());
            }
        }
        else
        {
            // Si no está en el agua y no se reveló, vuelve a su lugar original
            if (!yaEstaRevelada)
            {
                rectTransform.anchoredPosition = posicionInicial;
            }
        }
    }

    // --- ANIMACIÓN DE REVELADO ---

    private IEnumerator AnimacionRevelado()
    {
        float tiempo = 0f;
        Color colorInicial = Color.black;
        Color colorFinal = Color.white; // Color original/normal de la imagen

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            imagenFoto.color = Color.Lerp(colorInicial, colorFinal, tiempo / duracionAnimacion);
            yield return null;
        }

        imagenFoto.color = colorFinal;
        yaEstaRevelada = true;

        // Activa el botón para ver la foto en pantalla grande
        if (botonVerFoto != null)
        {
            botonVerFoto.SetActive(true);
        }
    }

    // --- MÉTODOS PARA LOS BOTONES (VER / CERRAR) ---

    // Asignar al OnClick del "BotonVerFoto"
    public void AbrirFotoDetallada()
    {
        if (popUpVistaDetallada != null)
        {
            popUpVistaDetallada.SetActive(true);
        }
    }

    // Asignar al OnClick del botón "Cerrar" dentro del PopUp
    public void CerrarFotoDetallada()
    {
        if (popUpVistaDetallada != null)
        {
            popUpVistaDetallada.SetActive(false);
        }
    }
}