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
        }

        if (dialogoAProcesar != null)
        {
            DialogoManager_Definitivo.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
            DialogoManager_Definitivo.Instance.OnDialogoFinalizado += OnDialogoFinalizado;

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

            // 1. Guardar primero las banderas para que el sistema reconozca que ya se poseen
            if (dialogo.banderaACompletar != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaACompletar);
            }

            if (dialogo.banderaExtra != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaExtra);
            }

            // 2. Gestionar las notificaciones de UI según corresponda
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
                // Si no hay habitación por desbloquear, llama directamente al resumen
                notificacionResumen.MostrarNotificacion();
            }
        }
        else if (notificacionResumen != null)
        {
            // En caso de diálogo por defecto
            notificacionResumen.MostrarNotificacion();
        }
    }
} //cambio para probar