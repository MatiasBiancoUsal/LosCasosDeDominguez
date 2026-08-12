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

        [Tooltip("ScriptableObject con el contenido del diálogo.")]
        public DialogoSistema dialogo;

        [Tooltip("Opcional: Bandera necesaria para habilitar este diálogo (ej: tener la lámpara).")]
        public GameFlag banderaRequerida;
    }

    [Header("Lista de diálogos priorizados (evaluados de arriba a abajo)")]
    [SerializeField] private CondicionDialogo[] dialogosPosibles;

    [Header("Diálogo por defecto (se repite cuando se agotaron los anteriores)")]
    [SerializeField] private DialogoSistema dialogoPorDefecto;

    private DetectorHover hover;
    private CondicionDialogo? dialogoSeleccionadoActual;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (hover != null && hover.MouseEstaEncima && Keyboard.current.iKey.wasPressedThisFrame)
        {
            EvaluarYIniciarDialogo();
        }
    }

    private void EvaluarYIniciarDialogo()
    {
        if (DialogoManager.Instance == null || GameStateManager.Instance == null)
        {
            Debug.LogError("[ActivarDialogo] Faltan los Managers (DialogoManager o GameStateManager) en la escena.");
            return;
        }

        DialogoSistema dialogoAProcesar = null;
        dialogoSeleccionadoActual = null;

        foreach (var cond in dialogosPosibles)
        {
            // 1. Si la bandera que este diálogo otorgaría YA fue completada, pasamos al siguiente
            if (cond.banderaACompletar != null && GameStateManager.Instance.TieneBandera(cond.banderaACompletar))
            {
                continue;
            }

            // 2. Si este diálogo requiere una bandera previa que el jugador AÚN NO TIENE, pasamos al siguiente
            if (cond.banderaRequerida != null && !GameStateManager.Instance.TieneBandera(cond.banderaRequerida))
            {
                continue;
            }

            // Si supera ambas pruebas, este es el diálogo correspondiente
            dialogoAProcesar = cond.dialogo;
            dialogoSeleccionadoActual = cond;
            break;
        }

        // Si no cumple ninguna condición o la lista se agotó, usa el por defecto
        if (dialogoAProcesar == null)
        {
            dialogoAProcesar = dialogoPorDefecto;
        }

        if (dialogoAProcesar != null)
        {
            DialogoManager.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
            DialogoManager.Instance.OnDialogoFinalizado += OnDialogoFinalizado;

            DialogoManager.Instance.IniciarDialogo(dialogoAProcesar);
        }
    }

    private void OnDialogoFinalizado()
    {
        if (DialogoManager.Instance != null)
        {
            DialogoManager.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
        }

        // Al terminar, si el diálogo asignaba una bandera, se registra en el GameStateManager
        if (dialogoSeleccionadoActual.HasValue && dialogoSeleccionadoActual.Value.banderaACompletar != null)
        {
            GameStateManager.Instance.GuardarBandera(dialogoSeleccionadoActual.Value.banderaACompletar);
        }
    }
}