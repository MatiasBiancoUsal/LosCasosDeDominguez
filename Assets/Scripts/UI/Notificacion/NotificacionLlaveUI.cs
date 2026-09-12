using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NotificacionLlaveUI : MonoBehaviour
{
    public static NotificacionLlaveUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelNotificacion;
    [SerializeField] private TMP_Text textoNotificacionUI;
    public AudioClip clip;

    private NotificacionResumen resumenPendiente = null;
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
        if (!estaNotificando) return;

        if (Keyboard.current != null && (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame))
        {
            CerrarNotificacion();
        }
    }

    public void MostrarNotificacion(string nombreHabitacion, NotificacionResumen resumenOpcional = null)
    {
        resumenPendiente = resumenOpcional;

        SonidosDeUI.instance.Play(clip);

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
        estaNotificando = false;

        if (panelNotificacion != null)
        {
            panelNotificacion.SetActive(false);
        }

        if (resumenPendiente != null)
        {
            resumenPendiente.MostrarNotificacion();
            resumenPendiente = null;
        }
    }
}