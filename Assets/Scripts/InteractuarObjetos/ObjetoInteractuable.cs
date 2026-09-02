using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Persistencia")]
    [SerializeField] private GameFlag banderaAOtorgar;

    [Tooltip("Si está activado, el objeto dejará de responder cuando la bandera ya haya sido obtenida.")]
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
        if (banderaAOtorgar == null || GameStateManager.Instance == null)
            return;

        if (!GameStateManager.Instance.TieneBandera(banderaAOtorgar))
            return;

        // Si el objeto es recolectable y se debe destruir al conseguir la flag,
        // desaparece al cargar la escena.
        if (destruirAlInteractuar)
        {
            Destroy(gameObject);
            return;
        }

        // Solo bloqueamos la interacción si así lo indicamos desde el Inspector.
        if (bloquearSiYaTieneBandera)
        {
            yaFueCompletado = true;
        }
    }

    private void Update()
    {
        // Si ya completó su interacción y está configurado para bloquearse,
        // no responde nuevamente.
        if (yaFueCompletado)
            return;

        if (esperandoCierrePanelInfo)
        {
            if (Keyboard.current != null &&
                (Keyboard.current.xKey.wasPressedThisFrame ||
                 Keyboard.current.qKey.wasPressedThisFrame))
            {
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
                EjecutarInteraccion();
            }
        }
    }

    private void EjecutarInteraccion()
{
    if (banderaAOtorgar != null && GameStateManager.Instance != null)
    {
        // Solo otorgar la bandera si todavía no la tiene.
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

        // Los sospechosos manejan su propio cierre del panel.
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

        // Espera al siguiente frame para no procesar la tecla 'Q' de inmediato
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

        // SI DESTRUYES EL GAMEOBJECT, hazlo en el frame siguiente 
        // para asegurar que NotificacionLlaveUI procesó la llamada correctamente.
        if (destruirAlInteractuar)
        {
            Destroy(gameObject);
        }
    }
}