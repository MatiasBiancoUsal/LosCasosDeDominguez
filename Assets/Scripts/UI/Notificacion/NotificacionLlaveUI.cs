using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NotificacionLlaveUI : MonoBehaviour
{
    public static NotificacionLlaveUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelNotificacion;
    [SerializeField] private TMP_Text textoNotificacionUI;

    private bool estaNotificando = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (estaNotificando && Keyboard.current != null)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
            {
                CerrarNotificacion();
            }
        }
    }

    public void MostrarNotificacion(string nombreHabitacion)
    {
        if (textoNotificacionUI != null)
        {
            textoNotificacionUI.text = $"¡Ya podés entrar {nombreHabitacion}!";
        }

        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(true);
        }

        estaNotificando = true;
    }

    public void CerrarNotificacion()
    {
        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(false);
        }
        estaNotificando = false;
    }
}