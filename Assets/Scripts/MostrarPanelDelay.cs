using UnityEngine;
using System.Collections;

public class MostrarPanelDelay : MonoBehaviour
{
    [Header("Flag que activa el panel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Panel que debe cerrarse primero")]
    public GameObject panelAnterior;

    [Header("Delay después de cerrar el panel anterior")]
    [SerializeField] private float delayDespuesDeCerrar = 0.5f;

    private bool esperandoPanel = false;
    private bool panelMostrado = false;
    private bool esperandoDelay = false;

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
        if (esperandoPanel && !panelMostrado && !esperandoDelay)
        {
            if (panelAnterior == null || !panelAnterior.activeSelf)
            {
                StartCoroutine(MostrarConDelay());
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

    private IEnumerator MostrarConDelay()
    {
        esperandoDelay = true;

        Debug.Log("[MostrarPanelDelay] Panel anterior cerrado. Esperando " +
                  delayDespuesDeCerrar + " segundos...");

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Volvemos a comprobar por seguridad que el panel anterior
        // siga cerrado después del delay.
        if (panelAnterior != null && panelAnterior.activeSelf)
        {
            Debug.Log("[MostrarPanelDelay] El panel anterior volvió a abrirse. Cancelando.");
            esperandoDelay = false;
            yield break;
        }

        MostrarPanel();

        esperandoDelay = false;
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

        // Guardamos que este cartel ya apareció
        PlayerPrefs.SetInt(ClaveNotificacion, 1);
        PlayerPrefs.Save();

        panel.SetActive(true);

        esperandoPanel = false;
        panelMostrado = true;

        Debug.Log("[MostrarPanelDelay] Nuevo panel mostrado.");
    }
}