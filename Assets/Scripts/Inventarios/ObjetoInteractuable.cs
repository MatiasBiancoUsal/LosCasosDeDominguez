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

    [Header("Persistencia (PlayerPrefs)")]
    [Tooltip("Clave única para guardar si el jugador tiene este objeto.")]
    [SerializeField] private string clavePlayerPrefObjeto;

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

    // Detector de hover que ya está en este mismo GameObject
    private DetectorHover detectorHover;

    // Indica si este objeto abrió un panel
    private bool panelAbierto = false;


    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();

        if (detectorHover == null)
        {
            Debug.LogError(
                "ObjetoInteractuable necesita un DetectorHover en el mismo GameObject: "
                + gameObject.name
            );
        }
    }


    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // --------------------------------------------------
        // CERRAR PANEL
        // --------------------------------------------------

        if (panelAbierto && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CerrarPanelActual();
            return;
        }


        // --------------------------------------------------
        // COMPROBAR HOVER
        // --------------------------------------------------

        if (detectorHover == null || !detectorHover.MouseEstaEncima)
            return;


        // --------------------------------------------------
        // INTERACCIÓN CON Q
        // --------------------------------------------------

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q DETECTADA EN: " + gameObject.name);

            EjecutarInteraccion();
        }
    }


    private void EjecutarInteraccion()
    {
        // --------------------------------------------------
        // GUARDAR OBJETO EN PLAYERPREFS
        // --------------------------------------------------

        if (!string.IsNullOrEmpty(clavePlayerPrefObjeto))
        {
            PlayerPrefs.SetInt(clavePlayerPrefObjeto, 1);
            PlayerPrefs.Save();

            Debug.Log(
                "Guardado en PlayerPrefs: "
                + clavePlayerPrefObjeto
                + " = 1"
            );
        }


        // --------------------------------------------------
        // DETERMINAR TIPO DE INTERACCIÓN
        // --------------------------------------------------

        switch (tipoDeObjeto)
        {
            case TipoInteraccion.DesbloquearPista:

                AbrirPista();

                break;


            case TipoInteraccion.DesbloquearSospechoso:

                AbrirInspeccionSospechoso();

                break;
        }
    }


    // ======================================================
    // PISTA
    // ======================================================

    private void AbrirPista()
    {
        // Desbloquear el objeto en el inventario
        if (itemEnInventario != null)
        {
            itemEnInventario.Desbloquear();
        }


        // Comprobar que existe el manager
        if (ObjetoInfoManager.Instance == null)
        {
            Debug.LogWarning(
                "No existe ObjetoInfoManager en la escena."
            );

            return;
        }


        // Comprobar que existe la información del objeto
        if (datosDelObjeto == null)
        {
            Debug.LogWarning(
                "No hay PistasScriptable asignado en: "
                + gameObject.name
            );

            return;
        }


        // Abrir panel de información
        ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);

        panelAbierto = true;

        Debug.Log(
            "Abriendo información de pista: "
            + datosDelObjeto.nombreObjeto
        );
    }


    // ======================================================
    // SOSPECHOSO
    // ======================================================

    private void AbrirInspeccionSospechoso()
    {
        // Desbloquear en inventario
        if (itemEnInventario != null)
        {
            itemEnInventario.Desbloquear();
        }


        // Caso especial de la lámpara
        if (esLampara)
        {
            EstadoJuego.tieneLampara = true;

            Debug.Log("Lámpara desbloqueada.");
        }


        // Comprobar referencias
        if (inspectionManager == null)
        {
            Debug.LogWarning(
                "No hay InspectionManager asignado en: "
                + gameObject.name
            );

            return;
        }


        if (suspectData == null)
        {
            Debug.LogWarning(
                "No hay SuspectData asignado en: "
                + gameObject.name
            );

            return;
        }


        // Asignar sospechoso
        inspectionManager.SetSuspect(suspectData);

        // El InspectionManager ya abre el panel,
        // pero mantenemos esta referencia para poder cerrarlo.
        panelAbierto = true;

        Debug.Log(
            "Abriendo inspección de: "
            + suspectData.suspectName
        );
    }


    // ======================================================
    // CERRAR PANEL
    // ======================================================

    private void CerrarPanelActual()
    {
        panelAbierto = false;


        if (tipoDeObjeto == TipoInteraccion.DesbloquearPista)
        {
            if (ObjetoInfoManager.Instance != null)
            {
                ObjetoInfoManager.Instance.CerrarPanel();
            }

            Debug.Log("Panel de pista cerrado.");
        }


        else if (tipoDeObjeto == TipoInteraccion.DesbloquearSospechoso)
        {
            if (inspectionPanel != null)
            {
                inspectionPanel.SetActive(false);
            }

            Debug.Log("Panel de inspección cerrado.");
        }
    }


    // ======================================================
    // CIERRE DESDE UI
    // ======================================================

    public void ForzarCierreDesdeUI()
    {
        CerrarPanelActual();
    }
}