using UnityEngine;
using UnityEngine.EventSystems;

public class EfectoHoverCarta : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración del Zoom")]
    [SerializeField] private Vector3 escalaHover = new Vector3(1.15f, 1.15f, 1f);
    [SerializeField] private float velocidadTransicion = 10f;

    private Vector3 escalaOriginal;
    private Vector3 escalaObjetivo;

    private void Awake()
    {
        escalaOriginal = transform.localScale;
        escalaObjetivo = escalaOriginal;
    }

    private void Update()
    {
        // Interpola suavemente la escala hacia la escala objetivo
        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.deltaTime * velocidadTransicion);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = Vector3.Scale(escalaOriginal, escalaHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal;
    }

    private void OnDisable()
    {
        // Restablece la escala por si el objeto se desactiva en medio del hover
        transform.localScale = escalaOriginal;
    }
}
