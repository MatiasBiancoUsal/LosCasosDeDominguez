using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DetectorHover))]
public class Puertas : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Nombre exacto de la escena a la que lleva esta puerta")]
    [SerializeField] private string nombreDeLaEscena;

    [Header("Requisito de Desbloqueo")]
    [Tooltip("Pista/Bandera necesaria para abrir esta puerta. Si se deja vacío, la puerta está abierta por defecto.")]
    [SerializeField] private GameFlag banderaRequerida;

    [Header("UI / Paneles Flotantes Sobre la Puerta")]
    [Tooltip("Panel o botón que se activa cuando la puerta ESTÁ desbloqueada.")]
    [SerializeField] private GameObject panelPuertaDesbloqueada;

    [Tooltip("Panel opcional que se activa cuando la puerta ESTÁ bloqueada (ej: cartel 'Puerta Cerrada').")]
    [SerializeField] private GameObject panelPuertaBloqueada;

    [Header("Estado actual (Lectura en Inspector)")]
    [SerializeField] private bool desbloqueado = false;

    private DetectorHover detectorHover;
    private bool panelActivo = false;

    private void Awake()
    {
        // Obtenemos el DetectorHover que ya tiene adjunto esta misma puerta
        detectorHover = GetComponent<DetectorHover>();
    }

    private void Start()
    {
        OcultarPaneles();
        VerificarEstadoDesbloqueo();
    }

    private void Update()
    {
        if (detectorHover == null) return;

        // 1. Si el mouse está sobre la puerta Y se presiona la tecla N
        if (detectorHover.MouseEstaEncima && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            // Si el panel de 'Desbloqueado' ya estaba abierto y se presiona N de vuelta -> Pasa de nivel
            if (panelActivo && desbloqueado)
            {
                CargarNivel();
            }
            else
            {
                IntentarInteraccionar();
            }
        }

        // 2. Si el mouse SALE del objeto, cerramos los paneles para que no queden flotando
        if (!detectorHover.MouseEstaEncima && panelActivo)
        {
            OcultarPaneles();
        }
    }

    /// <summary>
    /// Le consulta directamente al GameStateManager si la bandera necesaria está conseguida.
    /// </summary>
    public bool VerificarEstadoDesbloqueo()
    {
        if (GameStateManager.Instance != null)
        {
            desbloqueado = GameStateManager.Instance.TieneBandera(banderaRequerida);
        }
        else
        {
            Debug.LogWarning("[Puertas] No se encontró el GameStateManager en la escena.");
            desbloqueado = (banderaRequerida == null);
        }

        return desbloqueado;
    }

    private void IntentarInteraccionar()
    {
        VerificarEstadoDesbloqueo();

        if (desbloqueado)
        {
            Debug.Log($"[Puertas] Puerta ABIERTA: {gameObject.name}");

            if (panelPuertaDesbloqueada != null)
            {
                OcultarPaneles();
                panelPuertaDesbloqueada.SetActive(true);
                panelActivo = true;
            }
            else
            {
                // Si no configuraron panel flotante, entra de una a la siguiente escena
                CargarNivel();
            }
        }
        else
        {
            string nombrePista = banderaRequerida != null ? banderaRequerida.name : "Desconocida";
            Debug.Log($"[Puertas] Puerta BLOQUEADA. Te falta la bandera: {nombrePista}");

            if (panelPuertaBloqueada != null)
            {
                OcultarPaneles();
                panelPuertaBloqueada.SetActive(true);
                panelActivo = true;
            }
        }
    }

    /// <summary>
    /// Función pública para asignar al botón UI si prefieren hacerle click con el mouse
    /// </summary>
    public void CargarNivelBoton()
    {
        VerificarEstadoDesbloqueo();

        if (desbloqueado)
        {
            CargarNivel();
        }
        else
        {
            Debug.LogWarning("[Puertas] Intentaste cargar nivel pero la puerta sigue bloqueada.");
        }
    }

    public void OcultarPaneles()
    {
        panelActivo = false;

        if (panelPuertaDesbloqueada != null)
            panelPuertaDesbloqueada.SetActive(false);

        if (panelPuertaBloqueada != null)
            panelPuertaBloqueada.SetActive(false);
    }

    private void CargarNivel()
    {
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            SceneManager.LoadScene(nombreDeLaEscena);
        }
        else
        {
            Debug.LogError($"[Puertas] El campo nombreDeLaEscena está vacío en {gameObject.name}.");
        }
    }
}