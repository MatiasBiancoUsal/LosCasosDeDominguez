using UnityEngine;

public class DetectiveResaltadoUI : MonoBehaviour
{
    [Header("Banderas de la Historia")]
    [SerializeField] private GameFlag[] banderasRequeridas;
    [SerializeField] private GameFlag banderaCompletadaDetective;

    [Header("Referencias Visuales")]
    [SerializeField] private GameObject iconoResaltado;
    [SerializeField] private GameObject botonConversarUI;

    private DetectorHover hover;
    private bool debeEstarActivo;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += EvaluarBanderas;
            Debug.Log($"[DetectiveResaltadoUI] {gameObject.name} se suscribió a OnBanderaObtenida.");
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= EvaluarBanderas;
        }
    }

    private void Start()
    {
        ActualizarEstadoVisual();
    }

    private void Update()
    {
        if (botonConversarUI != null)
        {
            bool mostrarBoton = debeEstarActivo && (hover != null && hover.MouseEstaEncima);
            if (botonConversarUI.activeSelf != mostrarBoton)
            {
                botonConversarUI.SetActive(mostrarBoton);
            }
        }
    }

    private void EvaluarBanderas(GameFlag banderaRecienObtenida)
    {
        Debug.Log($"[DetectiveResaltadoUI] {gameObject.name} recibió la notificación de bandera: {banderaRecienObtenida?.name}");
        ActualizarEstadoVisual();
    }

    public void ActualizarEstadoVisual()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning($"[DetectiveResaltadoUI] GameStateManager.Instance es NULL en {gameObject.name}");
            return;
        }

        bool condicionRequeridaCumplida = CumpleTodasLasBanderas();
        bool yaHabloConDetective = banderaCompletadaDetective != null && GameStateManager.Instance.TieneBandera(banderaCompletadaDetective);

        debeEstarActivo = condicionRequeridaCumplida && !yaHabloConDetective;

        Debug.Log($"[DetectiveResaltadoUI] Evaluando {gameObject.name} -> CumpleRequeridas: {condicionRequeridaCumplida} | YaHablo: {yaHabloConDetective} | ResultadoActivo: {debeEstarActivo}");

        if (iconoResaltado != null)
        {
            iconoResaltado.SetActive(debeEstarActivo);
        }
        else
        {
            Debug.LogError($"[DetectiveResaltadoUI] ¡Falta asignar 'iconoResaltado' en {gameObject.name}!");
        }
    }

    private bool CumpleTodasLasBanderas()
    {
        if (banderasRequeridas == null || banderasRequeridas.Length == 0) return true;

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag == null) continue;

            bool laTiene = GameStateManager.Instance.TieneBandera(flag);
            Debug.Log($"[DetectiveResaltadoUI] Comprobando si tiene '{flag.name}': {laTiene}");

            if (!laTiene)
            {
                return false;
            }
        }

        return true;
    }
}