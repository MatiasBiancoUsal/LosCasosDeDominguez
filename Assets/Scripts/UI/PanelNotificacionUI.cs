using UnityEngine;
using System.Collections;

public class MostrarPanelConFlag : MonoBehaviour
{
    [Header("Flag que activa el cartel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Tiempo que permanece visible")]
    public float duracion = 3f;

    private Coroutine rutinaPanel;

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
            return;

        // Por si la flag ya había sido obtenida anteriormente
        // antes de cargar esta escena.
        if (GameStateManager.Instance.TieneBandera(flagNecesaria) &&
            !NotificacionYaMostrada())
        {
            MostrarCartel();
        }

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