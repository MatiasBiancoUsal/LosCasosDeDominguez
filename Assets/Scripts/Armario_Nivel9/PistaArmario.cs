using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PistaArmario : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración de Pista y Flag")]
    [SerializeField] private PistasScriptable datosDelObjeto;
    [SerializeField] private GameFlag banderaObjeto;
    [SerializeField] private float tiempoSostenidoQ = 3f;

    [Header("Referencias UI Carga")]
    [SerializeField] private Slider sliderCargaQ;
    [SerializeField] private ArmarioMinigame managerMinijuego;

    [Header("UI Costado Pantalla (Objeto Recolectado)")]
    [SerializeField] private GameObject panelObjetoCostado;   
    [SerializeField] private Image imagenFotoCostado;         
    [SerializeField] private GameObject cartelNombreGris;    
    [SerializeField] private TextMeshProUGUI textoNombreGris; 

    private float tiempoPresionado = 0f;
    private bool mouseSobrePista = false;

    private void Start()
    {
        if (managerMinijuego == null)
            managerMinijuego = FindFirstObjectByType<ArmarioMinigame>();

        OcultarCartelGris();

        if (GameStateManager.Instance != null && banderaObjeto != null)
        {
            if (GameStateManager.Instance.TieneBandera(banderaObjeto))
            {
                ActivarFotoCostado();
                gameObject.SetActive(false);
                return;
            }
        }

        ResetearCarga();
    }

    private void Update()
    {
        if (mouseSobrePista && Input.GetKey(KeyCode.Q))
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
        if (GameStateManager.Instance != null && banderaObjeto != null)
        {
            GameStateManager.Instance.GuardarBandera(banderaObjeto);
        }

        ActivarFotoCostado();
        ResetearCarga();

        if (managerMinijuego != null)
        {
            managerMinijuego.PistaRecolectada(gameObject);
        }
    }

    private void ActivarFotoCostado()
    {
        if (panelObjetoCostado != null)
            panelObjetoCostado.SetActive(true);

        if (datosDelObjeto != null)
        {
            if (imagenFotoCostado != null && datosDelObjeto.imagenObjeto != null)
                imagenFotoCostado.sprite = datosDelObjeto.imagenObjeto;

            if (textoNombreGris != null)
                textoNombreGris.text = datosDelObjeto.nombreObjeto;
        }

        OcultarCartelGris();
    }

    public void OnPointerEnter(PointerEventData eventData) => mouseSobrePista = true;

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseSobrePista = false;
        ResetearCarga();
    }

    public void MostrarCartelGris()
    {
        if (cartelNombreGris != null)
            cartelNombreGris.SetActive(true);
    }

    public void OcultarCartelGris()
    {
        if (cartelNombreGris != null)
            cartelNombreGris.SetActive(false);
    }

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