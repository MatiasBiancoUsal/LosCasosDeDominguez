using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DetectorHover))]
public class Puertas : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Nombre exacto de la escena en Build Settings")]
    [SerializeField] private string nombreDeLaEscena;

    [Header("Requisito de Desbloqueo")]
    [Tooltip("Bandera/Pista necesaria para abrir esta puerta.")]
    [SerializeField] private GameFlag banderaRequerida;

    [Header("Bandera al Interactuar (Opcional)")]
    [Tooltip("Si asignás una bandera, se otorgará al interactuar con esta puerta.")]
    [SerializeField] private GameFlag banderaAlInteractuar;

    [Header("Feedback Visual Bloqueado (Opcional)")]
    [Tooltip("Solo si querés mostrar un cartel si la puerta ESTÁ CERRADA.")]
    [SerializeField] private GameObject mensajePuertaCerrada;

    [Tooltip("Tiempo en segundos que permanecerá visible el mensaje de puerta cerrada.")]
    [SerializeField] private float tiempoMensaje = 3f;

    public AudioClip[] puertaSonidos;

    private DetectorHover detectorHover;
    private bool cargandoEscena = false;
    private Coroutine rutinaOcultarMensaje;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (detectorHover == null || cargandoEscena) return;

        // Respuesta INMEDIATA: Al apretar N sobre la puerta
        if (detectorHover.MouseEstaEncima &&
            Keyboard.current != null &&
            Keyboard.current.nKey.wasPressedThisFrame)
        {
            IntentarEntrar();
        }
    }

    private void IntentarEntrar()
    {
        // 0. Otorgar SIEMPRE la bandera al interactuar (incluso si la puerta está cerrada)
        if (banderaAlInteractuar != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(banderaAlInteractuar);
            Debug.Log($"[Puertas] Intento registrado. Bandera guardada: {banderaAlInteractuar.name}");
        }

        // 1. Chequeamos bandera requerida (llave)
        bool desbloqueado = (banderaRequerida == null);

        if (!desbloqueado && GameStateManager.Instance != null)
        {
            desbloqueado = GameStateManager.Instance.TieneBandera(banderaRequerida);
        }

        // 2. Si está desbloqueada, pasamos a la siguiente escena
        if (desbloqueado)
        {
            cargandoEscena = true;
            Debug.Log($"[Puertas] Transición hacia: {nombreDeLaEscena}");
            SceneManager.LoadSceneAsync(nombreDeLaEscena);
        }
        else
        {
            Debug.Log("[Puertas] Puerta bloqueada. Falta pista.");
            MostrarMensajeBloqueado();
        }
    }

    private void MostrarMensajeBloqueado()
    {
        if (mensajePuertaCerrada == null) return;

        mensajePuertaCerrada.SetActive(true);

        if (rutinaOcultarMensaje != null)
        {
            StopCoroutine(rutinaOcultarMensaje);
        }

        //rutinaOcultarMensaje = StartCoroutine(RutinaOcultarMensaje());
    }

    /*private IEnumerator RutinaOcultarMensaje()
    {
        yield return new WaitForSeconds(tiempoMensaje);

        if (mensajePuertaCerrada != null)
        {
            mensajePuertaCerrada.SetActive(false);
        }
    }*/
}