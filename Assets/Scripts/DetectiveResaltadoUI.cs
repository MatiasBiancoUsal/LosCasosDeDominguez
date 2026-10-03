using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DetectiveResaltadoUI : MonoBehaviour
{
    [Header("Banderas de la Historia")]
    [Tooltip("Las banderas necesarias que deben cumplirse para activar este resaltado.")]
    [SerializeField] private GameFlag[] banderasRequeridas;

    [Tooltip("Bandera que se obtendrá al hablar y que desactivará el resaltado.")]
    [SerializeField] private GameFlag banderaCompletadaDetective;

    [Header("Referencias Visuales (UI)")]
    [Tooltip("CanvasGroup del ícono 'i' (ej: Boton Interrogatorio).")]
    [SerializeField] private CanvasGroup iconoResaltadoCG;

    [Tooltip("CanvasGroup del botón de hablar flotante (Opcional, puedes dejarlo en None si no usas).")]
    [SerializeField] private CanvasGroup botonConversarCG;

    [Tooltip("Transform del ícono para la animación de flotación (ej: Boton Interrogatorio).")]
    [SerializeField] private Transform contenedorIcono;

    [Header("Iluminación 2D")]
    [SerializeField] private Light2D luzSpotDetective;
    [SerializeField] private float intensidadNormal = 1.2f;
    [SerializeField] private float intensidadHover = 2.2f;
    [SerializeField] private float velocidadTransicionLuz = 4f;

    [Header("Efecto Titileo / Latido de Luz")]
    [SerializeField] private bool usarTitileo = true;
    [SerializeField] private float velocidadTitileo = 2f;   
    [SerializeField] private float variacionMinima = 0.3f;   
    [SerializeField] private float variacionMaxima = 1.4f;   

    [Header("Animación y Feedback")]
    [SerializeField] private float velocidadFade = 5f;
    [SerializeField] private float amplitudFlotacion = 0.08f;
    [SerializeField] private float velocidadFlotacion = 2.5f;

    [Header("Componentes de Interacción")]
    [Tooltip("Arrastra aquí el BoxCollider2D o CapsuleCollider2D del detective.")]
    [SerializeField] private Collider2D miCollider;

    private DetectorHover hover;
    private bool debeEstarActivo;
    private Vector3 posicionInicialIcono;
    private Coroutine fadeIconoRoutine;
    private Coroutine fadeBotonRoutine;
    private Coroutine rutinaLuz;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
        if (miCollider == null) miCollider = GetComponent<Collider2D>();

        if (contenedorIcono != null)
        {
            posicionInicialIcono = contenedorIcono.localPosition;
        }

        ConfigurarCanvasGroupInicial(iconoResaltadoCG);
        ConfigurarCanvasGroupInicial(botonConversarCG);

        if (luzSpotDetective != null)
        {
            luzSpotDetective.enabled = true;
            luzSpotDetective.intensity = 0f;
        }
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
        if (!debeEstarActivo) return;

        bool mouseEncima = hover != null && hover.MouseEstaEncima;

        if (contenedorIcono != null)
        {
            float nuevoY = posicionInicialIcono.y + Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
            contenedorIcono.localPosition = new Vector3(posicionInicialIcono.x, nuevoY, posicionInicialIcono.z);
        }

        if (botonConversarCG != null)
        {
            float targetAlphaBoton = mouseEncima ? 1f : 0f;
            if (!Mathf.Approximately(botonConversarCG.alpha, targetAlphaBoton))
            {
                ActualizarFade(ref fadeBotonRoutine, botonConversarCG, targetAlphaBoton);
            }
        }

        if (luzSpotDetective != null)
        {
            float baseIntensidad = mouseEncima ? intensidadHover : intensidadNormal;

            if (usarTitileo)
            {
                float factorRuido = Mathf.PerlinNoise(Time.time * velocidadTitileo, 0f);

                float multiplicadorLuz = Mathf.Lerp(variacionMinima, variacionMaxima, factorRuido);

                if (Random.value > 0.93f)
                {
                    multiplicadorLuz *= 0.2f; 
                }

                baseIntensidad *= multiplicadorLuz;
            }

            luzSpotDetective.intensity = baseIntensidad;
        }
    }

    private void EvaluarBanderas(GameFlag banderaRecienObtenida)
    {
        Debug.Log($"[DetectiveResaltadoUI] {gameObject.name} recibió notificación de bandera: {banderaRecienObtenida?.name}");
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

        if (iconoResaltadoCG != null)
        {
            float targetAlphaIcono = debeEstarActivo ? 1f : 0f;
            ActualizarFade(ref fadeIconoRoutine, iconoResaltadoCG, targetAlphaIcono);
        }

        if (luzSpotDetective != null)
        {
            float targetIntensidad = debeEstarActivo ? intensidadNormal : 0f;
            if (rutinaLuz != null) StopCoroutine(rutinaLuz);
            rutinaLuz = StartCoroutine(TransicionLuzRoutine(targetIntensidad));
        }

        if (miCollider != null) miCollider.enabled = debeEstarActivo;
        if (hover != null) hover.enabled = debeEstarActivo;
    }

    private bool CumpleTodasLasBanderas()
    {
        if (banderasRequeridas == null || banderasRequeridas.Length == 0) return true;

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag == null) continue;

            bool laTiene = GameStateManager.Instance.TieneBandera(flag);
            if (!laTiene) return false;
        }

        return true;
    }

    private void ConfigurarCanvasGroupInicial(CanvasGroup cg)
    {
        if (cg == null) return;
        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    private void ActualizarFade(ref Coroutine rutinaActual, CanvasGroup cg, float targetAlpha)
    {
        if (rutinaActual != null) StopCoroutine(rutinaActual);
        rutinaActual = StartCoroutine(FadeRoutine(cg, targetAlpha));
    }

    private IEnumerator FadeRoutine(CanvasGroup cg, float targetAlpha)
    {
        while (!Mathf.Approximately(cg.alpha, targetAlpha))
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlpha, velocidadFade * Time.deltaTime);
            yield return null;
        }

        cg.alpha = targetAlpha;
        cg.blocksRaycasts = targetAlpha > 0.5f;
        cg.interactable = targetAlpha > 0.5f;
    }

    private IEnumerator TransicionLuzRoutine(float targetIntensidad)
    {
        while (!Mathf.Approximately(luzSpotDetective.intensity, targetIntensidad))
        {
            luzSpotDetective.intensity = Mathf.MoveTowards(
                luzSpotDetective.intensity,
                targetIntensidad,
                velocidadTransicionLuz * Time.deltaTime
            );
            yield return null;
        }
        luzSpotDetective.intensity = targetIntensidad;
    }
}