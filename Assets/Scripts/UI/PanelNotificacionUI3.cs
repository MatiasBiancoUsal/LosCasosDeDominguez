using UnityEngine;
using System.Collections;

public class MPanelNotificacionUI3 : MonoBehaviour
{
    [Header("Flags necesarias para activar el cartel")]
    public GameFlag flag1;
    public GameFlag flag2;
    public GameFlag flag3;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Tiempo que permanece visible")]
    public float duracion = 3f;

    private Coroutine rutinaPanel;

    // Clave para recordar que este cartel ya fue mostrado
    private string ClaveNotificacion
    {
        get
        {
            if (flag1 == null || flag2 == null || flag3 == null)
                return "";

            return "NotificacionMostrada_3Flags_" +
                   flag1.Id + "_" +
                   flag2.Id + "_" +
                   flag3.Id;
        }
    }

    private void Start()
    {
        if (GameStateManager.Instance == null)
            return;

        // Por si las 3 flags ya habían sido obtenidas
        // antes de cargar esta escena.
        ComprobarFlags();

        // Escuchamos cuando se obtiene una nueva bandera.
        GameStateManager.Instance.OnBanderaObtenida += AlObtenerBandera;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= AlObtenerBandera;
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        // Cada vez que se obtiene una flag,
        // comprobamos si ahora ya están las 3.
        ComprobarFlags();
    }

    private void ComprobarFlags()
    {
        if (GameStateManager.Instance == null)
            return;

        // Si el cartel ya fue mostrado, no hacemos nada.
        if (NotificacionYaMostrada())
            return;

        bool tieneFlag1 = flag1 != null &&
                          GameStateManager.Instance.TieneBandera(flag1);

        bool tieneFlag2 = flag2 != null &&
                          GameStateManager.Instance.TieneBandera(flag2);

        bool tieneFlag3 = flag3 != null &&
                          GameStateManager.Instance.TieneBandera(flag3);

        // Solo muestra el panel cuando están las 3.
        if (tieneFlag1 && tieneFlag2 && tieneFlag3)
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

        if (rutinaPanel != null)
        {
            StopCoroutine(rutinaPanel);
        }

        rutinaPanel = StartCoroutine(MostrarPanel());
    }

    private IEnumerator MostrarPanel()
    {
        panel.SetActive(true);

        yield return new WaitForSeconds(duracion);

        panel.SetActive(false);

        rutinaPanel = null;
    }
}