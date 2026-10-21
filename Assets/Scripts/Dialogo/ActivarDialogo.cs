using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivarDialogo : MonoBehaviour
{
    [Serializable]
    public struct CondicionDialogo
    {
        [Tooltip("Bandera que se guardará en el GameStateManager cuando este diálogo termine.")]
        public GameFlag banderaACompletar;

        [Tooltip("Nombre de la habitación a desbloquear si esta bandera otorga un acceso (ej: Cocina).")]
        public string nombreHabitacionADesbloquear;

        [Tooltip("Segunda bandera opcional que también se guardará cuando este diálogo termine.")]
        public GameFlag banderaExtra;

        [Tooltip("ScriptableObject con el contenido del diálogo.")]
        public DialogoSistema dialogo;

        [Tooltip("Opcional: Bandera necesaria para habilitar este diálogo (ej: tener la lámpara).")]
        public GameFlag banderaRequerida;
    }

    [Header("Lista de diálogos priorizados (evaluados de arriba a abajo)")]
    [SerializeField] private CondicionDialogo[] dialogosPosibles;

    [Header("Diálogo por defecto (se repite cuando se agotaron los anteriores)")]
    [SerializeField] private DialogoSistema dialogoPorDefecto;

    [Header("Secuencia Opcional (Resumen)")]
    [Tooltip("Si se asigna, al terminar este diálogo y cerrar las fotos se abrirá el panel de resumen.")]
    [SerializeField] private NotificacionResumen notificacionResumen;

    [Header("Configuración Analytics")]
    [SerializeField] public string nombreHabitacionActual;
    [Tooltip("Marcar esta casilla SOLO en los personajes de la Escena Final.")]
    [SerializeField] private bool esInterrogatorioFinal = false;

    private DetectorHover hover;
    private CondicionDialogo? dialogoSeleccionadoActual;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (hover != null && hover.MouseEstaEncima)
            {
                EvaluarYIniciarDialogo();
            }
        }
    }

    private void EvaluarYIniciarDialogo()
    {
        if (DialogoManager_Definitivo.Instance == null) return;

        DialogoSistema dialogoAProcesar = null;
        dialogoSeleccionadoActual = null;
        bool esRepetido = false;

        foreach (var cond in dialogosPosibles)
        {
            if (GameStateManager.Instance != null)
            {
                if (cond.banderaACompletar != null &&
                    GameStateManager.Instance.TieneBandera(cond.banderaACompletar))
                {
                    continue;
                }

                if (cond.banderaRequerida != null &&
                    !GameStateManager.Instance.TieneBandera(cond.banderaRequerida))
                {
                    continue;
                }
            }

            dialogoAProcesar = cond.dialogo;
            dialogoSeleccionadoActual = cond;
            break;
        }

        if (dialogoAProcesar == null)
        {
            dialogoAProcesar = dialogoPorDefecto;
            esRepetido = true;
        }

        if (dialogoAProcesar != null)
        {
            DialogoManager_Definitivo.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
            DialogoManager_Definitivo.Instance.OnDialogoFinalizado += OnDialogoFinalizado;

            if (esInterrogatorioFinal)
            {
                // Print de prueba para el Interrogatorio Final
                print($"[Analytics] Interrogatorio Final -> Sospechoso: {gameObject.name}, Diálogo: {dialogoAProcesar.name}");

                // SACAR DE COMENTARIO ASI SE ENVIA EL DATO
                // EnviarDato.Singleton.EventoInterrogacionFinal(gameObject.name, dialogoAProcesar.name);
            }
            else
            {
                // Print de prueba para Diálogo Normal
                print($"[Analytics] Enviando Dialogo -> Personaje: {gameObject.name}, Habitación: {nombreHabitacionActual}, Diálogo: {dialogoAProcesar.name}, Repetido: {esRepetido}");

                // SACAR DE COMENTARIO ASI SE ENVIA EL DATO
                // EnviarDato.Singleton.EventoDialogo(gameObject.name, nombreHabitacionActual, dialogoAProcesar.name, esRepetido);
            }

            DialogoManager_Definitivo.Instance.IniciarDialogo(dialogoAProcesar);
        }
        else
        {
            Debug.LogWarning("[ActivarDialogo] No hay ningún diálogo asignado ni por defecto para mostrar.");
        }
    }

    private void OnDialogoFinalizado()
    {
        if (DialogoManager_Definitivo.Instance != null)
        {
            DialogoManager_Definitivo.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
        }

        if (GameStateManager.Instance != null && dialogoSeleccionadoActual.HasValue)
        {
            CondicionDialogo dialogo = dialogoSeleccionadoActual.Value;

            if (dialogo.banderaACompletar != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaACompletar);
            }

            if (dialogo.banderaExtra != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaExtra);
            }

            if (!string.IsNullOrEmpty(dialogo.nombreHabitacionADesbloquear))
            {
                if (dialogo.banderaACompletar != null)
                {
                    GameStateManager.Instance.RegistrarHabitacionDesbloqueada(dialogo.banderaACompletar, dialogo.nombreHabitacionADesbloquear);
                }

                if (NotificacionLlaveUI.Instance != null)
                {
                    NotificacionLlaveUI.Instance.MostrarNotificacion(dialogo.nombreHabitacionADesbloquear, notificacionResumen);
                }
            }
            else if (notificacionResumen != null)
            {
                notificacionResumen.MostrarNotificacion();
            }
        }
        else if (notificacionResumen != null)
        {
            notificacionResumen.MostrarNotificacion();
        }
    }
}