using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
        if (banderasRequeridas == null || banderasRequeridas.Count == 0) return true;

        if (GameStateManager.Instance == null)
        {
            Debug.LogError("[NotificacionResumen] No se encontró el GameStateManager en escena.");
            return false;
        }

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag != null && !GameStateManager.Instance.TieneBandera(flag))
            {
                return false; 
            }
        }

        return true;
    }

    public void MostrarNotificacion()
    {
        if (!TieneTodasLasBanderas())
        {
            Debug.Log("[NotificacionResumen] Aún no se cumplen las banderas necesarias para activar el resumen.");
            return;
        }

        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(true);
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