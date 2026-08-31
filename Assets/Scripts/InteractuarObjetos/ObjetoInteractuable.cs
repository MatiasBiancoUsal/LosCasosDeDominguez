using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Persistencia")]
    [SerializeField] private GameFlag banderaAOtorgar;

    [Header("Comportamiento Recolectable")]
    [SerializeField] private bool destruirAlInteractuar = false;

    [Header("Habitación")]
    [SerializeField] private string nombreHabitacionDesbloqueada;

    private DetectorHover detectorHover;
    private IAccionInteractuable accionEspecifica;
    private bool esperandoCierrePanelInfo = false;
    private bool esPrimeraInteraccion = false;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
        accionEspecifica = GetComponent<IAccionInteractuable>();
    }

    private void Start()
    {
        ComprobarSiYaFueRecogido();
    }

    private void ComprobarSiYaFueRecogido()
    {
        if (banderaAOtorgar != null && GameStateManager.Instance != null)
        {
            if (GameStateManager.Instance.TieneBandera(banderaAOtorgar) && destruirAlInteractuar)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (esperandoCierrePanelInfo)
        {
            if (Keyboard.current != null && (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.qKey.wasPressedThisFrame))
            {
                Debug.Log("[Paso 3] Se presionó la tecla para cerrar la info y mostrar la notificación.");
                MostrarNotificacionFinal();
            }
            return;
        }

        if (Keyboard.current == null) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (detectorHover != null && detectorHover.MouseEstaEncima)
            {
                Debug.Log("[Paso 1] Presionaste la Q y el ratón está encima del objeto.");
                EjecutarInteraccion();
            }
            else
            {
                Debug.LogWarning("[Check] Presionaste la Q, pero 'MouseEstaEncima' es FALSE (el detector Hover no reconoce el ratón).");
            }
        }
    }

    private void EjecutarInteraccion()
    {
        Debug.Log("[Paso 2] Ejecutando interacción...");

        if (banderaAOtorgar != null && GameStateManager.Instance != null)
        {
            if (!GameStateManager.Instance.TieneBandera(banderaAOtorgar))
            {
                esPrimeraInteraccion = true;
                GameStateManager.Instance.GuardarBandera(banderaAOtorgar);

                if (!string.IsNullOrEmpty(nombreHabitacionDesbloqueada))
                {
                    GameStateManager.Instance.RegistrarHabitacionDesbloqueada(banderaAOtorgar, nombreHabitacionDesbloqueada);
                }
            }
        }

        if (accionEspecifica != null)
        {
            Debug.Log("[Paso 2.1] Objeto con 'IAccionInteractuable' (AccionPista) detectado.");
            accionEspecifica.EjecutarAccion();
            esperandoCierrePanelInfo = true;
        }
        else
        {
            Debug.Log("[Paso 2.2] Objeto sin panel previo. Mostrando notificación directa.");
            MostrarNotificacionFinal();
        }
    }

    private void MostrarNotificacionFinal()
    {
        esperandoCierrePanelInfo = false;

        // Dispara la notificación solo la primera vez si se le asignó una habitación
        if (esPrimeraInteraccion && NotificacionLlaveUI.Instance != null && !string.IsNullOrEmpty(nombreHabitacionDesbloqueada))
        {
            NotificacionLlaveUI.Instance.MostrarNotificacion(nombreHabitacionDesbloqueada);
            esPrimeraInteraccion = false;
        }

        // Si la casilla está marcada, se destruye el objeto
        if (destruirAlInteractuar)
        {
            Destroy(gameObject);
        }
    }
}