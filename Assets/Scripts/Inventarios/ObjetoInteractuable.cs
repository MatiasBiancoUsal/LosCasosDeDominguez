using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoInteractuable : MonoBehaviour
{
    public enum TipoInteraccion { DesbloquearPista, DesbloquearSospechoso }

    [Header("Configuración del Objeto")]
    [SerializeField] private TipoInteraccion tipoDeObjeto;

    [Header("Referencia al Ítem de la UI (Para quitar el gris)")]
    [SerializeField] private ItemInventarioUI itemEnInventario;

    [Header("Referencias a los Nuevos Controladores")]
    [SerializeField] private InventarioPistas scriptPistas;
    [SerializeField] private InventarioSospechosos scriptSospechosos;

    private bool mouseEncima = false;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if ((scriptPistas != null && scriptPistas.EstaAbierto()) ||
            (scriptSospechosos != null && scriptSospechosos.EstaAbierto()))
        {
            return;
        }

        if (mouseEncima && Keyboard.current.wKey.wasPressedThisFrame)
        {
            switch (tipoDeObjeto)
            {
                case TipoInteraccion.DesbloquearPista:
                    if (scriptPistas != null) scriptPistas.Abrir();
                    if (itemEnInventario != null) itemEnInventario.Desbloquear();
                    break;

                case TipoInteraccion.DesbloquearSospechoso:
                    if (scriptSospechosos != null) scriptSospechosos.Abrir();
                    if (itemEnInventario != null) itemEnInventario.Desbloquear();
                    break;
            }
        }
    }
    private void OnMouseEnter() => mouseEncima = true;
    private void OnMouseExit() => mouseEncima = false;
}
