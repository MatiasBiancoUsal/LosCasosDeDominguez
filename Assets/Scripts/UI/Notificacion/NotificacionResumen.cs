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

    private bool estaActiva = false;

    private void Awake()
    {
        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(false); // Se mantiene oculto al inicio
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

    public void MostrarNotificacion()
    {
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
        else
        {
            Debug.LogError("[NotificacionResumen] No asignaste el nombre de la escena de resumen.");
        }
    }
}