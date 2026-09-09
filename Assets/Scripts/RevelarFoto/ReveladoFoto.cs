using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class ReveladoFoto : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referencias UI")]
    public RectTransform zonaAgua;
    public Image imagenFoto;
    public GameObject botonVerFoto;
    public GameObject popUpVistaDetallada;
    public GameObject botonSalir; 

    [Header("Configuración de Salida")]
    [Tooltip("Nombre de la escena a la que te llevará el botón de salir.")]
    public string nombreEscenaSalida;

    [Header("Ajustes de Revelado")]
    public float duracionAnimacion = 2.0f;
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

        if (imagenFoto != null)
        {
            imagenFoto.color = Color.black;
        }

        if (botonVerFoto != null) botonVerFoto.SetActive(false);
        if (popUpVistaDetallada != null) popUpVistaDetallada.SetActive(false);
    }

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
            if (!yaEstaRevelada)
            {
                rectTransform.anchoredPosition = posicionInicial;
            }
        }
    }

    private IEnumerator AnimacionRevelado()
    {
        float tiempo = 0f;
        Color colorInicial = Color.black;
        Color colorFinal = Color.white;

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            imagenFoto.color = Color.Lerp(colorInicial, colorFinal, tiempo / duracionAnimacion);
            yield return null;
        }

        imagenFoto.color = colorFinal;
        yaEstaRevelada = true;

        if (botonVerFoto != null)
        {
            botonVerFoto.SetActive(true);
        }

        if (botonSalir != null)
        {
            botonSalir.SetActive(true); 
        }
    }

    public void AbrirFotoDetallada()
    {
        if (popUpVistaDetallada != null)
        {
            popUpVistaDetallada.SetActive(true);
        }
    }

    public void CerrarFotoDetallada()
    {
        if (popUpVistaDetallada != null)
        {
            popUpVistaDetallada.SetActive(false);
        }
    }
    public void SalirAEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaSalida))
        {
            SceneManager.LoadScene(nombreEscenaSalida);
        }
    }
}