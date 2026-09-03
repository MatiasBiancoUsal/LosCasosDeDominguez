using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoLimite = 15f;
    [SerializeField] private int totalPistasRequeridas = 3;

    [Header("Referencias UI Escena Minijuego")]
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [SerializeField] private GameObject panelNotificacionFinal;
    [SerializeField] private TextMeshProUGUI textoNotificacion;

    private float tiempoRestante;
    private bool juegoActivo = true;
    private List<string> pistasEncontradas = new List<string>();

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        if (panelNotificacionFinal != null) panelNotificacionFinal.SetActive(false);
    }

    private void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        if (textoTiempo != null)
        {
            textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString() + "s";
        }

        if (tiempoRestante <= 0)
        {
            TiempoAgotado();
        }
    }

    public void PistaRecolectada(string nombrePista, GameObject objetoPista)
    {
        if (!juegoActivo) return;

        objetoPista.SetActive(false);
        if (!pistasEncontradas.Contains(nombrePista))
        {
            pistasEncontradas.Add(nombrePista);
        }

        if (pistasEncontradas.Count >= totalPistasRequeridas)
        {
            GanarMinijuego();
        }
    }

    private void GanarMinijuego()
    {
        juegoActivo = false;

        // Guardar flags de forma global
        ArmarioInteractuable.minijuegoCompletado = true;
        ArmarioInteractuable.pistasObtenidas = pistasEncontradas.ToArray();

        // Mostrar panel con el resumen
        if (panelNotificacionFinal != null) panelNotificacionFinal.SetActive(true);

        if (textoNotificacion != null)
        {
            string mensaje = "¡Conseguiste todas las pistas!\n\nEncontraste:\n";
            foreach (string pista in pistasEncontradas)
            {
                mensaje += "- " + pista + "\n";
            }
            textoNotificacion.text = mensaje;
        }
    }

    private void TiempoAgotado()
    {
        juegoActivo = false;
        // Cerrar escena directamente si pierde por tiempo
        CerrarEscenaMinijuego();
    }

    // Conecta esta función al botón "Aceptar / Continuar" del Panel de Notificación
    public void CerrarEscenaMinijuego()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }
}