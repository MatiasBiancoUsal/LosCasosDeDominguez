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

    private bool mouseEncima;
    private bool panelAbierto = false;

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (!mouseEncima) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q apretada sobre: " + gameObject.name);

            switch (tipoDeObjeto)
            {
                case TipoInteraccion.DesbloquearPista:

                    // Desbloquea el ítem en el inventario
                    if (itemEnInventario != null)
                    {
                        itemEnInventario.Desbloquear();
                    }

                    // Abre o cierra el panel de información antiguo
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
                        Debug.LogError("CRÍTICO: No se encuentra ObjetoInfoManager en la escena.");
                    }

                    break;

                case TipoInteraccion.DesbloquearSospechoso:

                    // Desbloquea al sospechoso en el inventario
                    if (itemEnInventario != null)
                    {
                        itemEnInventario.Desbloquear();
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
        Debug.Log("Mouse ENTRÓ a: " + gameObject.name);
    }

    private void OnMouseExit()
    {
        mouseEncima = false;
        Debug.Log("Mouse SALIÓ de: " + gameObject.name);
    }
}