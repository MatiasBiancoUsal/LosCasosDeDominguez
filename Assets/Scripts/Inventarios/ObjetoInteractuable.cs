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

    [Header("Ficha de Datos (ScriptableObject)")]
    [SerializeField] private PistasScriptable datosDelObjeto;

    [Header("Ítem correspondiente en el inventario")]
    [SerializeField] private ItemInventarioUI itemEnInventario;

    private bool mouseEncima;
    private bool panelAbierto = false; // Llevamos el control individual de este objeto

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (!mouseEncima) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q apretada sobre: " + gameObject.name);

            // 1. Desbloqueo de inventario (Tu código original)
            if (itemEnInventario != null)
            {
                itemEnInventario.Desbloquear();
            }

            // 2. Lógica de abrir / cerrar el panel de inspección
            if (ObjetoInfoManager.Instance != null)
            {
                if (!panelAbierto)
                {
                    ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);
                    panelAbierto = true;
                }
                else
                {
                    ObjetoInfoManager.Instance.CerrarPanel();
                    panelAbierto = false;
                }
            }
            else
            {
                // ¡Si sale este error en rojo, encontramos al culpable!
                Debug.LogError("CRÍTICO: ¡No se encuentra el ObjetoInfoManager en la escena!");
            }
        }
    }

    private void OnMouseEnter()
    {
        mouseEncima = true;
        Debug.Log("Mouse ENTRÓ a: " + gameObject.name);
    }

    private void OnMouseExit()
    {
        mouseEncima = false;
        Debug.Log("Mouse SALIÓ de: " + gameObject.name);
    }
}