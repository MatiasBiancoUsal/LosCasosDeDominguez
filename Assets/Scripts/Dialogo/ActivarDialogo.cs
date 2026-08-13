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

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {

            if (hover != null && hover.MouseEstaEncima)
            {
                EvaluarYIniciarDialogo();
            }
            else
            {
                Debug.Log("[ActivarDialogo] Presionaste la 'I', pero el ratón NO está sobre el personaje (Hover es false).");
            }
        }
    }

    private void EvaluarYIniciarDialogo()
    {
        if (DialogoManager.Instance == null)
        {
            Debug.LogError("[ActivarDialogo] Falta DialogoManager.Instance en la escena.");
            return;
        }

        DialogoSistema dialogoAProcesar = null;
        dialogoSeleccionadoActual = null;

        foreach (var cond in dialogosPosibles)
        {
            // Si el GameStateManager existe, evaluamos banderas. Si no existe, ignoramos las banderas para evitar bloqueos.
            if (GameStateManager.Instance != null)
            {
                if (cond.banderaACompletar != null && GameStateManager.Instance.TieneBandera(cond.banderaACompletar))
                {
                    continue;
                }

                if (cond.banderaRequerida != null && !GameStateManager.Instance.TieneBandera(cond.banderaRequerida))
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
            DialogoManager.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
            DialogoManager.Instance.OnDialogoFinalizado += OnDialogoFinalizado;

            DialogoManager.Instance.IniciarDialogo(dialogoAProcesar);
        }
        else
        {
            Debug.LogWarning("[ActivarDialogo] No hay ningún diálogo asignado ni por defecto para mostrar.");
        }
    }

    private void OnDialogoFinalizado()
    {
        if (DialogoManager.Instance != null)
        {
            DialogoManager.Instance.OnDialogoFinalizado -= OnDialogoFinalizado;
        }

        if (GameStateManager.Instance != null && dialogoSeleccionadoActual.HasValue && dialogoSeleccionadoActual.Value.banderaACompletar != null)
        {
            GameStateManager.Instance.GuardarBandera(dialogoSeleccionadoActual.Value.banderaACompletar);
        }
    }
}