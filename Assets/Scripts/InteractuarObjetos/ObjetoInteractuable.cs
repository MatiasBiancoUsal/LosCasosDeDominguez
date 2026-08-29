using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Persistencia")]
    [Tooltip("Bandera que se otorga al interactuar con este objeto.")]
    [SerializeField] private GameFlag banderaAOtorgar;

    [Header("Comportamiento Recolectable")]
    [Tooltip("Si está marcado, el objeto desaparecerá al interactuar y no reaparecerá si la bandera ya fue guardada.")]
    [SerializeField] private bool destruirAlInteractuar = false;

    [Header("Configuración de Notificación (Opcional)")]
    [Tooltip("Panel UI que se mostrará. Si se deja vacío, el objeto simplemente desaparecerá.")]
    [SerializeField] private GameObject panelNotificacion;

    private DetectorHover detectorHover;
    private IAccionInteractuable accionEspecifica;
    private bool estaNotificando = false;

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
                // Si la bandera ya fue obtenida y el objeto es destructible, se destruye al cargar la escena
                if (destruirAlInteractuar)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void Update()
    {
        if (estaNotificando)
        {
            if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            {
                CerrarNotificacionYDestruir();
            }
            return;
        }

        if (Keyboard.current == null || detectorHover == null || !detectorHover.MouseEstaEncima)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            EjecutarInteraccion();
        }
    }

    private void EjecutarInteraccion()
    {
        // 1. Guardar la Bandera en el GameStateManager
        if (banderaAOtorgar != null)
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.GuardarBandera(banderaAOtorgar);
            }
            else
            {
                Debug.LogError("[ObjetoInteractuable] No existe GameStateManager en la escena.");
            }
        }

        // 2. Ejecutar acción específica si la tiene
        if (accionEspecifica != null)
        {
            accionEspecifica.EjecutarAccion();
        }

        // 3. Manejar desaparición / notificación
        if (destruirAlInteractuar)
        {
            if (panelNotificacion != null)
            {
                // Si tiene panel asignado (como la Llave), lo muestra y espera segundo toque de Q
                panelNotificacion.SetActive(true);
                estaNotificando = true;

                if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;
                if (TryGetComponent<SpriteRenderer>(out var rend)) rend.enabled = false;
            }
            else
            {
                // Si NO tiene panel (como la Lámpara), desaparece inmediatamente
                Destroy(gameObject);
            }
        }
    }

    private void CerrarNotificacionYDestruir()
    {
        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(false);
        }

        Destroy(gameObject);
    }
}