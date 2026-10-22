using UnityEngine;
using UnityEngine.InputSystem;

public class AccionSospechoso : MonoBehaviour, IAccionInteractuable
{
    [Header("Configuraci�n de Inspecci�n")]
    [SerializeField] private SuspectData suspectData;
    [SerializeField] private InspectionManager inspectionManager;
    [SerializeField] private GameObject inspectionPanel;

    private bool panelAbierto = false;

    private void Update()
    {
        // Cerrar panel con la tecla X
        if (panelAbierto && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CerrarPanel();
        }
    }

    public void EjecutarAccion()
    {
        if (inspectionManager == null || suspectData == null)
        {
            Debug.LogWarning($"[AccionSospechoso] Falta InspectionManager o SuspectData en: {gameObject.name}");
            return;
        }

        inspectionManager.SetSuspect(suspectData);
        panelAbierto = true;

        inspectionManager.SetSuspect(suspectData);
        panelAbierto = true;

        // ANALYTICS - Maia
        string nombreSospechoso = suspectData.suspectName;

        print($"[Analytics] InspeccionarSospechoso -> Nombre del sospechoso: {nombreSospechoso}");

        // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (Maia)
        EnviarDato.Singleton.EventoInspeccionarSospechoso(nombreSospechoso);
    }

    public void CerrarPanel()
    {
        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
        }
        panelAbierto = false;
    }

    public void ForzarCierreDesdeUI()
    {
        CerrarPanel();
    }
}