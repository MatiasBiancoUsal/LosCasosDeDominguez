using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PistaArmario : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración de Pista y Flag")]
    [SerializeField] private PistasScriptable datosDelObjeto;
    [SerializeField] private GameFlag banderaObjeto; // Tu Scriptable Object GameFlag
    [SerializeField] private float tiempoSostenidoQ = 3f;

    [Header("Referencias UI")]
    [SerializeField] private Slider sliderCargaQ;
    [SerializeField] private ArmarioMinigame managerMinijuego;

    private float tiempoPresionado = 0f;
    private bool mouseEncima = false;

    private void Start()
    {
        if (managerMinijuego == null)
            managerMinijuego = FindFirstObjectByType<ArmarioMinigame>();

        // Si ya guardamos esta GameFlag antes en GameStateManager, la pista se oculta
        if (GameStateManager.Instance != null && banderaObjeto != null)
        {
            if (GameStateManager.Instance.TieneBandera(banderaObjeto))
            {
                gameObject.SetActive(false);
                return;
            }
        }

        ResetearCarga();
    }

    private void Update()
    {
        if (mouseEncima && Input.GetKey(KeyCode.Q))
        {
            if (sliderCargaQ != null) sliderCargaQ.gameObject.SetActive(true);

            tiempoPresionado += Time.deltaTime;

            if (sliderCargaQ != null)
                sliderCargaQ.value = tiempoPresionado / tiempoSostenidoQ;

            if (tiempoPresionado >= tiempoSostenidoQ)
                CompletarRecoleccion();
        }
        else if (tiempoPresionado > 0)
        {
            ResetearCarga();
        }
    }

    private void CompletarRecoleccion()
    {
        // Guardamos la GameFlag con tu GameStateManager global
        if (GameStateManager.Instance != null && banderaObjeto != null)
        {
            GameStateManager.Instance.GuardarBandera(banderaObjeto);
        }

        // Mostramos el panel de información si usas ObjetoInfoManager
        if (ObjetoInfoManager.Instance != null && datosDelObjeto != null)
        {
            ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);
        }

        ResetearCarga();

        if (managerMinijuego != null)
        {
            managerMinijuego.PistaRecolectada(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => mouseEncima = true;
    public void OnPointerExit(PointerEventData eventData) { mouseEncima = false; ResetearCarga(); }

    private void ResetearCarga()
    {
        tiempoPresionado = 0f;
        if (sliderCargaQ != null)
        {
            sliderCargaQ.value = 0f;
            sliderCargaQ.gameObject.SetActive(false);
        }
    }
}