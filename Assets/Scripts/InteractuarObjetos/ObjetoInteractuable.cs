using System.Collections;
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

    [Header("Secuencia Opcional (Resumen)")]
    [Tooltip("Si se asigna, este panel de resumen se mostrará al finalizar la interacción.")]
    [SerializeField] private NotificacionResumen notificacionResumen;

    private DetectorHover detectorHover;
    private IAccionInteractuable accionEspecifica;
    private bool esperandoCierrePanelInfo = false;
    private bool esPrimeraInteraccion = false;
    private bool yaFueCompletado = false; // Bloquea re-interacciones no deseadas

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
            if (GameStateManager.Instance.TieneBandera(banderaAOtorgar))
            {
                if (destruirAlInteractuar)
                {
                    Destroy(gameObject);
                }
                else
                {
                    // Si ya tenía la bandera de antes, marcamos como completado para evitar reactivaciones
                    yaFueCompletado = true;
                }
            }
        }
    }

    private void Update()
    {
        // Si el objeto ya completó su interacción (ej. Ramona), ignora cualquier otro Q o Hover
        if (yaFueCompletado) return;

        if (esperandoCierrePanelInfo)
        {
            if (Keyboard.current != null && (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.qKey.wasPressedThisFrame))
            {
                StartCoroutine(SecuenciaNotificacionFinal());
            }
            return;
        }

        if (Keyboard.current == null) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (detectorHover != null && detectorHover.MouseEstaEncima)
            {
                EjecutarInteraccion();
            }
        }
    }

    private void EjecutarInteraccion()
    {
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
            accionEspecifica.EjecutarAccion();
            esperandoCierrePanelInfo = true;
        }
        else
        {
            StartCoroutine(SecuenciaNotificacionFinal());
        }
    }

    private IEnumerator SecuenciaNotificacionFinal()
    {
        esperandoCierrePanelInfo = false;
        yaFueCompletado = true; // Sella el objeto para que no vuelva a responder a la Q con el Mouse encima

        yield return null; // Espera un frame para evitar que la tecla Q de cierre active otra cosa

        if (esPrimeraInteraccion && NotificacionLlaveUI.Instance != null && !string.IsNullOrEmpty(nombreHabitacionDesbloqueada))
        {
            NotificacionLlaveUI.Instance.MostrarNotificacion(nombreHabitacionDesbloqueada, notificacionResumen);
            esPrimeraInteraccion = false;
        }
        else if (notificacionResumen != null)
        {
            notificacionResumen.MostrarNotificacion();
        }

        if (destruirAlInteractuar)
        {
            Destroy(gameObject);
        }
    }
}