using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoInteractuable : MonoBehaviour
{
    public enum TipoInteraccion
    {
        DesbloquearPista,
        DesbloquearSospechoso
    }

    [Header("Configuración del Objeto")]
    [SerializeField] private TipoInteraccion tipoDeObjeto;

    [Header("Ítem correspondiente en el inventario")]
    [SerializeField] private ItemInventarioUI itemEnInventario;

    [Header("Controladores de inspección")]
    [SerializeField] private InspeccionPistas scriptPistas;
    [SerializeField] private InspeccionSospechoso scriptSospechosos;

    private bool mouseEncima;

    private void Update()
    {
        if (Keyboard.current == null) return;

        bool algunPanelAbierto =
            (scriptPistas != null && scriptPistas.EstaAbierto()) ||
            (scriptSospechosos != null && scriptSospechosos.EstaAbierto());

        if (algunPanelAbierto) return;

        if (!mouseEncima) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q detectada sobre: " + gameObject.name);

            if (itemEnInventario != null)
            {
                itemEnInventario.Desbloquear();
            }
            else
            {
                Debug.LogWarning("No hay ItemInventarioUI asignado en " + gameObject.name);
            }

            switch (tipoDeObjeto)
            {
                case TipoInteraccion.DesbloquearPista:

                    Debug.Log("Abriendo inspección de pista");

                    if (scriptPistas != null)
                    {
                        scriptPistas.Abrir();
                    }
                    else
                    {
                        Debug.LogWarning("No hay InspeccionPistas asignado");
                    }

                    break;

                case TipoInteraccion.DesbloquearSospechoso:

                    Debug.Log("Abriendo inspección de sospechoso");

                    if (scriptSospechosos != null)
                    {
                        scriptSospechosos.Abrir();
                    }
                    else
                    {
                        Debug.LogWarning("No hay InspeccionSospechoso asignado");
                    }

                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        mouseEncima = true;
        Debug.Log("Mouse sobre: " + gameObject.name);
    }

    private void OnMouseExit()
    {
        mouseEncima = false;
    }
}