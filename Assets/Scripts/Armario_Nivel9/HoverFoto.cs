using UnityEngine;
using UnityEngine.EventSystems;

public class HoverFoto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject cartelNombreGris;

    private void Start()
    {
        if (cartelNombreGris != null)
            cartelNombreGris.SetActive(false);
    }

    // Al pasar el mouse sobre la foto juntada
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cartelNombreGris != null)
            cartelNombreGris.SetActive(true);
    }

    // Al quitar el mouse de la foto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (cartelNombreGris != null)
            cartelNombreGris.SetActive(false);
    }
}