using UnityEngine;

public class DetectorHover : MonoBehaviour
{
    [Header("Indicador visual")]
    [SerializeField] private GameObject detectorHover;
    [SerializeField] private GameObject botonInspeccion;
    [SerializeField] private GameObject botonInterrogatorio;

    [Header("Configuración")]
    [SerializeField] private LayerMask interactableLayer;

    private bool mouseEstaEncima;

    public bool MouseEstaEncima => mouseEstaEncima;

    private void Start()
    {
        OcultarIndicadores();
    }

    private void Update()
    {
        DetectarHover();
    }

    private void DetectarHover()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

        bool sobreMi = false;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            // Comprobar que pertenece a la layer Interactable
            if (((1 << hit.gameObject.layer) & interactableLayer) == 0)
                continue;

            // Comprobar que el collider pertenece a este objeto
            if (hit.gameObject == gameObject)
            {
                sobreMi = true;
                break;
            }
        }

        if (sobreMi && !mouseEstaEncima)
        {
            EntrarHover();
        }
        else if (!sobreMi && mouseEstaEncima)
        {
            SalirHover();
        }
    }

    private void EntrarHover()
    {
        mouseEstaEncima = true;

        Debug.Log("HOVER EN: " + gameObject.name);

        MostrarIndicadores();
    }

    private void SalirHover()
    {
        mouseEstaEncima = false;

        OcultarIndicadores();
    }

    private void MostrarIndicadores()
    {
        if (detectorHover != null)
            detectorHover.SetActive(true);

        if (botonInspeccion != null)
            botonInspeccion.SetActive(true);

        if (botonInterrogatorio != null)
            botonInterrogatorio.SetActive(true);
    }

    private void OcultarIndicadores()
    {
        if (detectorHover != null)
            detectorHover.SetActive(false);
    }
}