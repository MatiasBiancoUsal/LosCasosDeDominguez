
using UnityEngine;

public class MostrarPanelConFlag : MonoBehaviour
{
    [Header("Flag que activa el cartel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar")]
    public GameObject panel;

    // Clave que usamos para recordar que este cartel ya apareció
    private string ClaveNotificacion
    {
        get
        {
            if (flagNecesaria == null)
                return "";

            return "NotificacionMostrada_" + flagNecesaria.Id;
        }
    }

    private void Start()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("[MostrarPanelConFlag] GameStateManager.Instance sigue siendo NULL en Start.");
            return;
        }

        // Log de control para ver qué lee el script apenas arranca
        bool tiene = GameStateManager.Instance.TieneBandera(flagNecesaria);
        bool yaMostrada = NotificacionYaMostrada();
        Debug.Log($"[MostrarPanelConFlag] Chequeando {flagNecesaria?.name} | TieneBandera: {tiene} | YaMostrada: {yaMostrada}");

        // Si la flag ya fue obtenida antes de cargar esta escena y no se mostró antes
        if (tiene && !yaMostrada)
        {
            MostrarCartel();
        }

        // Escuchamos cuando se obtiene una nueva bandera
        GameStateManager.Instance.OnBanderaObtenida += AlObtenerBandera;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= AlObtenerBandera;
        }
    }

    private void Update()
    {
        // Si el panel está activo y se presiona X, lo cerramos
        if (panel != null &&
            panel.activeSelf &&
            Input.GetKeyDown(KeyCode.X))
        {
            panel.SetActive(false);
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        if (bandera == flagNecesaria &&
            !NotificacionYaMostrada())
        {
            MostrarCartel();
        }
    }

    private bool NotificacionYaMostrada()
    {
        if (string.IsNullOrEmpty(ClaveNotificacion))
            return false;

        return PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1;
    }

    private void MostrarCartel()
    {
        if (panel == null)
            return;

        // Guardamos que esta notificación ya apareció.
        PlayerPrefs.SetInt(ClaveNotificacion, 1);
        PlayerPrefs.Save();

        // Mostramos el panel y lo dejamos abierto
        // hasta que el jugador presione X.
        panel.SetActive(true);
    }
}

