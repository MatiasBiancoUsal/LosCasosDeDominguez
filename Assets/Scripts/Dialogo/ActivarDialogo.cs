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
        if (DialogoManager_Definitivo.Instance == null)
        {
            Debug.LogError("[ActivarDialogo] Falta DialogoManager.Instance en la escena.");
            return;
        }

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

            // Primera bandera
            if (dialogo.banderaACompletar != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaACompletar);
            }

            // Segunda bandera opcional
            if (dialogo.banderaExtra != null)
            {
                GameStateManager.Instance.GuardarBandera(dialogo.banderaExtra);
            }
        }
    }
}