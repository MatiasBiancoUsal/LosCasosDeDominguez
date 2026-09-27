
using UnityEngine;

public class MostrarPanelDelay : MonoBehaviour
{
    [Header("Flag que activa el panel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Panel que debe cerrarse primero")]
    public GameObject panelAnterior;

    private bool esperandoPanel = false;
    private bool panelMostrado = false;

    // Clave para recordar que este cartel ya apareció
    private string ClaveNotificacion
    {
        get
        {
            if (flagNecesaria == null)
                return "";

            return "MostrarPanelDelay_" + flagNecesaria.Id;
        }
    }

    private void Start()
    {
        if (GameStateManager.Instance == null)
            return;

        // Si la flag ya fue obtenida anteriormente,
        // solo mostramos el panel si nunca fue mostrado antes.
        if (GameStateManager.Instance.TieneBandera(flagNecesaria) &&
            !PanelYaMostrado())
        {
            esperandoPanel = true;
        }

        // Escuchamos cuando se obtiene una nueva flag
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
        // Esperamos a que se cierre el panel anterior
        if (esperandoPanel && !panelMostrado)
        {
            if (panelAnterior == null || !panelAnterior.activeSelf)
            {
                MostrarPanel();
            }
        }

        // Cerrar este panel con X
        if (panelMostrado &&
            panel != null &&
            panel.activeSelf &&
            Input.GetKeyDown(KeyCode.X))
        {
            panel.SetActive(false);
            panelMostrado = false;
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        if (bandera == flagNecesaria &&
            !PanelYaMostrado())
        {
            esperandoPanel = true;
        }
    }

    private bool PanelYaMostrado()
    {
        if (string.IsNullOrEmpty(ClaveNotificacion))
            return false;

        return PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1;
    }

    private void MostrarPanel()
    {
        if (panel == null)
            return;

        // Guardamos que este panel ya apareció
        PlayerPrefs.SetInt(ClaveNotificacion, 1);
        PlayerPrefs.Save();

        panel.SetActive(true);

        esperandoPanel = false;
        panelMostrado = true;
    }
}

