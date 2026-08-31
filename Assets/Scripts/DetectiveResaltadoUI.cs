using UnityEngine;

public class DetectiveResaltadoUI : MonoBehaviour
{
    [Header("Condición para activarse")]
    [SerializeField] private GameFlag banderaRequeridaLlamada;

    [Tooltip("La bandera que te da el detective al terminar su diálogo para apagarlo.")]
    [SerializeField] private GameFlag banderaCompletadaDetective;

    [Header("Referencias Visuales")]
    [Tooltip("Objeto del sprite/brillo sobre la cabeza o cuerpo del detective.")]
    [SerializeField] private GameObject iconoResaltado;

    [Tooltip("El botón de UI o cartel que indica 'Presiona I para hablar' / Botón clicable.")]
    [SerializeField] private GameObject botonConversarUI;

    private DetectorHover hover;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += OnBanderaObtenida;
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= OnBanderaObtenida;
        }
    }

    private void Start()
    {
        ActualizarEstadoVisual();
    }

    private void Update()
    {
        bool estaListoParaHablar = DebeEstarActivo();

        if (botonConversarUI != null)
        {
            bool mostrarBoton = estaListoParaHablar && (hover != null && hover.MouseEstaEncima);

            if (botonConversarUI.activeSelf != mostrarBoton)
            {
                botonConversarUI.SetActive(mostrarBoton);
            }
        }
    }

    /// <summary>
    /// Evento disparado por el GameStateManager en cuanto se guarda cualquier bandera (ej. termina la llamada).
    /// </summary>
    private void OnBanderaObtenida(GameFlag flagObtenida)
    {
        ActualizarEstadoVisual();
    }

    public void ActualizarEstadoVisual()
    {
        bool activo = DebeEstarActivo();

        if (iconoResaltado != null)
        {
            iconoResaltado.SetActive(activo);
        }

        if (!activo && botonConversarUI != null)
        {
            botonConversarUI.SetActive(false);
        }
    }

    /// <summary>
    /// Comprueba si ya hiciste la llamada del Tano Y AÚN NO has completado la charla con el Detective.
    /// </summary>
    private bool DebeEstarActivo()
    {
        if (GameStateManager.Instance == null) return false;

        bool llamadaHecha = GameStateManager.Instance.TieneBandera(banderaRequeridaLlamada);

        bool yaHabloConDetective = banderaCompletadaDetective != null &&
                                   GameStateManager.Instance.TieneBandera(banderaCompletadaDetective);

        return llamadaHecha && !yaHabloConDetective;
    }
}