using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Requisitos para Interactuar")]
    [Tooltip("Si se asigna, el jugador debe tener esta bandera para poder interactuar con el objeto.")]
    [SerializeField] private GameFlag banderaRequerida;

    [Tooltip("Objeto o texto de advertencia que se muestra si el jugador NO tiene la bandera requerida.")]
    [SerializeField] private GameObject panelTextoBloqueado;

    [Header("Persistencia")]
    [SerializeField] private GameFlag banderaAOtorgar;

    [Tooltip("Si está activado, el objeto dejará de responder cuando la bandera a otorgar ya haya sido obtenida.")]
    [SerializeField] private bool bloquearSiYaTieneBandera = true;

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
    private bool yaFueCompletado = false;
    private bool mensajeBloqueoMostrado = false;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
        accionEspecifica = GetComponent<IAccionInteractuable>();
    }

    // Cambiado a IEnumerator para esperar 1 frame tras la carga de escena
    private IEnumerator Start()
    {
        yield return null; // Da tiempo al GameStateManager para inicializarse
        ComprobarSiYaFueRecogido();
    }

    private void ComprobarSiYaFueRecogido()
    {
        if (banderaAOtorgar == null || GameStateManager.Instance == null)
            return;

        if (!GameStateManager.Instance.TieneBandera(banderaAOtorgar))
            return;

        if (destruirAlInteractuar)
        {
            Destroy(gameObject);
            return;
        }

        if (bloquearSiYaTieneBandera)
        {
            yaFueCompletado = true;
        }
    }

    private void Update()
    {
        if (yaFueCompletado)
            return;

        if (esperandoCierrePanelInfo)
        {
            if (Keyboard.current != null &&
                (Keyboard.current.xKey.wasPressedThisFrame ||
                 Keyboard.current.qKey.wasPressedThisFrame))
            {

                if (panelTextoBloqueado != null)
                    panelTextoBloqueado.SetActive(false);

                StartCoroutine(SecuenciaNotificacionFinal());
            }

            return;
        }

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (detectorHover != null && detectorHover.MouseEstaEncima)
            {
                if (banderaRequerida != null && GameStateManager.Instance != null)
                {
                    if (!GameStateManager.Instance.TieneBandera(banderaRequerida))
                    {
                        Debug.Log($"No se puede interactuar. Falta la bandera: {banderaRequerida.name}");

                        if (panelTextoBloqueado != null)
                        {
                            panelTextoBloqueado.SetActive(true);
                            mensajeBloqueoMostrado = true;
                        }

                        return;
                    }
                }

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
                    GameStateManager.Instance.RegistrarHabitacionDesbloqueada(
                        banderaAOtorgar,
                        nombreHabitacionDesbloqueada
                    );
                }
            }
        }

        if (accionEspecifica != null)
        {
            accionEspecifica.EjecutarAccion();

            if (accionEspecifica is AccionSospechoso)
            {
                esperandoCierrePanelInfo = false;
            }
            else
            {
                esperandoCierrePanelInfo = true;
            }
        }
        else
        {
            StartCoroutine(SecuenciaNotificacionFinal());
        }
    }

    private IEnumerator SecuenciaNotificacionFinal()
    {
        esperandoCierrePanelInfo = false;

        yield return null;

        if (bloquearSiYaTieneBandera)
        {
            yaFueCompletado = true;
        }

        if (esPrimeraInteraccion &&
            NotificacionLlaveUI.Instance != null &&
            !string.IsNullOrEmpty(nombreHabitacionDesbloqueada))
        {
            NotificacionLlaveUI.Instance.MostrarNotificacion(
                nombreHabitacionDesbloqueada,
                notificacionResumen
            );

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