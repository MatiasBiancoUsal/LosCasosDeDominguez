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

    [Header("Feedback Visual Bloqueado (Opcional)")]
    [Tooltip("Solo si querés mostrar un cartel si la puerta ESTÁ CERRADA.")]
    [SerializeField] private GameObject mensajePuertaCerrada;

    [Tooltip("Tiempo en segundos que permanecerá visible el mensaje de puerta cerrada.")]
    [SerializeField] private float tiempoMensaje = 3f;

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
        if (detectorHover.MouseEstaEncima && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            IntentarEntrar();
        }
    }

    private void IntentarEntrar()
    {
        // 1. Chequeamos bandera
        bool desbloqueado = (banderaRequerida == null);
        if (!desbloqueado && GameStateManager.Instance != null)
        {
            desbloqueado = GameStateManager.Instance.TieneBandera(banderaRequerida);
        }

        // 2. Si está desbloqueada, pasamos INMEDIATAMENTE
        if (desbloqueado)
        {
            cargandoEscena = true;
            Debug.Log($"[Puertas] Transición inmediata hacia: {nombreDeLaEscena}");

            // Carga asíncrona: No congela la pantalla y responde al instante
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

        // Activamos el cartel
        mensajePuertaCerrada.SetActive(true);

        // Si ya había una cuenta regresiva en marcha, la reiniciamos
        if (rutinaOcultarMensaje != null)
        {
            StopCoroutine(rutinaOcultarMensaje);
        }

        rutinaOcultarMensaje = StartCoroutine(RutinaOcultarMensaje());
    }

    private IEnumerator RutinaOcultarMensaje()
    {
        yield return new WaitForSeconds(tiempoMensaje);

        if (mensajePuertaCerrada != null)
        {
            mensajePuertaCerrada.SetActive(false);
        }
    }
}