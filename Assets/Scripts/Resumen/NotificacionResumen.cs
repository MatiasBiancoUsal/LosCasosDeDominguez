using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NotificacionResumen : MonoBehaviour
{
    [Header("UI del Panel")]
    [SerializeField] private GameObject panelNotificacion;
    [SerializeField] private Button botonIrAResumen;

    [Header("Navegación")]
    [SerializeField] private string nombreEscenaResumen = "EscenaResumen";

    [Header("Requisitos de Banderas / Pistas")]
    [Tooltip("Lista de banderas requeridas. Si no se especifican, se mostrará siempre.")]
    [SerializeField] private List<GameFlag> banderasRequeridas = new List<GameFlag>();

    private bool estaActiva = false;

    private void Awake()
    {
        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(false);
        }

        if (botonIrAResumen != null)
        {
            botonIrAResumen.onClick.AddListener(IrAEscenaResumen);
        }
    }

    private void Update()
    {
        if (!estaActiva) return;

        if (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame))
        {
            IrAEscenaResumen();
        }
    }

    private bool TieneTodasLasBanderas()
    {
        if (banderasRequeridas == null || banderasRequeridas.Count == 0)
        {
            Debug.Log("[NotificacionResumen] La lista 'banderasRequeridas' está vacía. Se omiten comprobaciones.");
            return true;
        }

        if (GameStateManager.Instance == null)
        {
            Debug.LogError("[NotificacionResumen] CRÍTICO: No existe GameStateManager.Instance en la escena.");
            return false;
        }

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag != null)
            {
                bool tieneEstaFlag = GameStateManager.Instance.TieneBandera(flag);
                Debug.Log($"[NotificacionResumen] Evaluando flag '{flag.Id}' -> Resultado: {(tieneEstaFlag ? "POSEE LA FLAG" : "NO LA POSEE")}");

                if (!tieneEstaFlag)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void MostrarNotificacion()
    {
        // LOG 1: Confirma que la llamada al método llega hasta este script
        Debug.Log("<color=yellow>[NotificacionResumen] Método MostrarNotificacion() EJECUTADO.</color>");

        if (!TieneTodasLasBanderas())
        {
            // LOG 2: Falla la validación de banderas
            Debug.LogWarning("[NotificacionResumen] CANCELADO: Faltan banderas requeridas por cumplir.");
            return;
        }

        if (panelNotificacion != null)
        {
            // LOG 3: Confirmación de activación
            Debug.Log($"<color=green>[NotificacionResumen] ¡ÉXITO! Intentando activar GameObject: {panelNotificacion.name}</color>");
            panelNotificacion.SetActive(true);
        }
        else
        {
            // LOG 4: Variable de la UI no asignada en el Inspector
            Debug.LogError("[NotificacionResumen] ERROR: La variable 'panelNotificacion' está NULL en el Inspector.");
        }

        estaActiva = true;
    }

    public void IrAEscenaResumen()
    {
        if (!string.IsNullOrEmpty(nombreEscenaResumen))
        {
            SceneManager.LoadScene(nombreEscenaResumen);
        }
    }
}