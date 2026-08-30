using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; 

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Persistencia")]
    [SerializeField] private GameFlag banderaAOtorgar;

    [Header("Comportamiento Recolectable")]
    [SerializeField] private bool destruirAlInteractuar = false;

    [Header("Habitación")]
    [SerializeField] private string nombreHabitacionDesbloqueada;

    [Header("Referencias UI (Canvas)")]
    [SerializeField] private GameObject panelNotificacion;
    [Tooltip("El componente Text Mesh Pro donde se mostrará el mensaje.")]
    [SerializeField] private TMP_Text textoNotificacionUI;

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
            if (GameStateManager.Instance.TieneBandera(banderaAOtorgar) && destruirAlInteractuar)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (estaNotificando)
        {
            if (Keyboard.current != null && (Keyboard.current.qKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame))
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
        if (banderaAOtorgar != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(banderaAOtorgar);
            GameStateManager.Instance.RegistrarHabitacionDesbloqueada(banderaAOtorgar, nombreHabitacionDesbloqueada);
        }

        accionEspecifica?.EjecutarAccion();

        if (destruirAlInteractuar)
        {
            if (panelNotificacion != null)
            {
                if (textoNotificacionUI != null)
                {
                    textoNotificacionUI.text = $"¡Desbloqueaste: {nombreHabitacionDesbloqueada}!";
                }

                panelNotificacion.SetActive(true);
                estaNotificando = true;

                if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;
                if (TryGetComponent<SpriteRenderer>(out var rend)) rend.enabled = false;
            }
            else
            {
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