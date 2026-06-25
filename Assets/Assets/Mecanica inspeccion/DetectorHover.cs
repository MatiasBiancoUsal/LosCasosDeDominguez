using UnityEngine;

public class DetectorHover : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject botonFlotante;
    public LayerMask interactableLayer;

    private bool mouseEstaEncima = false;

    // Permite que otros scripts sepan si el mouse está sobre este objeto
    public bool MouseEstaEncima => mouseEstaEncima;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

        bool sobreMi = false;

        foreach (Collider2D hit in hits)
        {
            // Ignora todo lo que no sea Interactable
            if (((1 << hit.gameObject.layer) & interactableLayer) == 0)
                continue;

            if (hit.gameObject == gameObject)
            {
                sobreMi = true;
                break;
            }
        }

        // Entró al hover
        if (sobreMi && !mouseEstaEncima)
        {
            mouseEstaEncima = true;

            if (botonFlotante != null)
                botonFlotante.SetActive(true);
        }

        // Salió del hover
        if (!sobreMi && mouseEstaEncima)
        {
            mouseEstaEncima = false;

            if (botonFlotante != null)
                botonFlotante.SetActive(false);
        }

        // Si el mouse está encima y apretás Q
        if (mouseEstaEncima && Input.GetKeyDown(KeyCode.Q))
        {
            EjecutarInspeccion();
        }
    }

    private void EjecutarInspeccion()
    {
        Debug.Log("Abriendo inspección de objetos...");
    }
}