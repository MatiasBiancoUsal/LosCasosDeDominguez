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

        if (panelAbierto && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CerrarPanelActual();
            return; 
        }

        if (!mouseEncima) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            switch (tipoDeObjeto)
            {
                case TipoInteraccion.DesbloquearPista:

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
                            CerrarPanelActual();
                        }
                    }
                    break;

                case TipoInteraccion.DesbloquearSospechoso:

                    if (itemEnInventario != null)
                    {
                        itemEnInventario.Desbloquear();

                        if (esLampara)
                        {
                            EstadoJuego.tieneLampara = true;
                        }
                    }

                    if (inspectionManager != null && suspectData != null)
                    {

                        inspectionManager.SetSuspect(suspectData);

                        if (inspectionPanel != null)
                        {
                            inspectionPanel.SetActive(true);
                        }

                        panelAbierto = true;
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

    private void CerrarPanelActual()
    {
        panelAbierto = false;

        if (tipoDeObjeto == TipoInteraccion.DesbloquearPista && ObjetoInfoManager.Instance != null)
        {
            ObjetoInfoManager.Instance.CerrarPanel();
        }
        else if (tipoDeObjeto == TipoInteraccion.DesbloquearSospechoso && inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
        }
    }

    public void ForzarCierreDesdeUI()
    {
        CerrarPanelActual();
    }
}