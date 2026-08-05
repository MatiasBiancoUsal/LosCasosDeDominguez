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

    [Header("Sistema de Inspección")]
    [SerializeField] private SuspectData suspectData;
    [SerializeField] private InspectionManager inspectionManager;
    [SerializeField] private GameObject inspectionPanel;

    [Header("Objeto Especial")]
    [SerializeField] private bool esLampara = false;

    private bool mouseEncima;
    private bool panelAbierto = false;

    private void Update()
    {
        if (Keyboard.current == null) return;

        // 1. CERRAR CON LA TECLA 'X'
        // Funciona siempre que el panel esté abierto, sin importar dónde esté el mouse
        if (panelAbierto && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CerrarPanelActual();
            return; // Corta el Update aquí para evitar conflictos en el mismo fotograma
        }

        // 2. ABRIR CON LA TECLA 'Q'
        // Requiere estrictamente que el mouse esté posicionado sobre el objeto
        if (!mouseEncima) return;

        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Mouse sobre el objeto.");
            switch (tipoDeObjeto)
            {
                case TipoInteraccion.DesbloquearPista:

                    // Desbloquea el ítem en el inventario si está asignado
                    if (itemEnInventario != null)
                    {
                        itemEnInventario.Desbloquear();
                    }

                    if (ObjetoInfoManager.Instance != null)
                    {
                        if (!panelAbierto)
                        {
                            ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);
                            panelAbierto = true;
                        }
                        else
                        {
                            // Permite que presionar la Q de nuevo también lo cierre si sigues apuntando al objeto
                            CerrarPanelActual();
                        }
                    }
                    break;

                case TipoInteraccion.DesbloquearSospechoso:

                    // Desbloquea al sospechoso en el inventario si está asignado
                    if (itemEnInventario != null)
                    {
                         itemEnInventario.Desbloquear();

                    if (esLampara)
                    {
                        EstadoJuego.tieneLampara = true;
                        Debug.Log("Lámpara obtenida.");
                    }
                    }

                    Debug.Log("Sospechoso desbloqueado.");

                    // Abre el panel de inspección del sospechoso
                    if (inspectionManager != null && suspectData != null)
                    {
                        Debug.Log("Panel: " + inspectionPanel);
                        Debug.Log("Suspect: " + suspectData.name);
                        Debug.Log("Manager: " + inspectionManager.name);

                        inspectionManager.SetSuspect(suspectData);

                        if (inspectionPanel != null)
                        {
                            inspectionPanel.SetActive(true);
                        }

                        // IMPORTANTE: Ahora el script sabe que el panel de sospechoso está activo
                        panelAbierto = true;
                        Debug.Log("Inspeccionando a: " + suspectData.suspectName);
                    }
                    else
                    {
                        Debug.LogError("Falta asignar InspectionManager o SuspectData.");
                    }
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        mouseEncima = true;
    }

    private void OnMouseExit()
    {
        mouseEncima = false;
    }

    // Función interna encargada de apagar la UI según el tipo de objeto actual
    private void CerrarPanelActual()
    {
        panelAbierto = false;

        if (tipoDeObjeto == TipoInteraccion.DesbloquearPista && ObjetoInfoManager.Instance != null)
        {
            ObjetoInfoManager.Instance.CerrarPanel();
            Debug.Log("Panel de pista cerrado.");
        }
        else if (tipoDeObjeto == TipoInteraccion.DesbloquearSospechoso && inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
            Debug.Log("Panel de sospechoso cerrado.");
        }
    }

    // Función pública para que los botones de la UI (haciendo clic con el mouse)
    // puedan cerrar el panel y resetear el script correctamente
    public void ForzarCierreDesdeUI()
    {
        CerrarPanelActual();
    }
}