using UnityEngine;
using System.Collections;

public class MostrarPanelDelay2 : MonoBehaviour
{
    [Header("Flag que activa el panel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar (Este Panel / Panel 3)")]
    public GameObject panel;

    [Header("Paneles que deben estar cerrados primero")]
    public GameObject panelAnterior1;
    public GameObject panelAnterior2;

    [Header("Delay después de que se cierren ambos paneles")]
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
        // Esperamos a que AMBOS paneles anteriores estén cerrados (o inexistentes)
        if (esperandoPanel && !panelMostrado && !esperandoDelay)
        {
            bool primerPanelCerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
            bool segundoPanelCerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

            if (primerPanelCerrado && segundoPanelCerrado)
            {
                StartCoroutine(MostrarConDelay());
            }
        }

        // Cerrar este panel (Panel 3) con X
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

        Debug.Log("[MostrarPanelDelay] Paneles anteriores cerrados. Esperando " +
                  delayDespuesDeCerrar + " segundos...");

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Volvemos a comprobar por seguridad que ninguno de los dos
        // se haya reabierto durante el delay.
        bool primerPanelCerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
        bool segundoPanelCerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

        if (!primerPanelCerrado || !segundoPanelCerrado)
        {
            Debug.Log("[MostrarPanelDelay] Uno de los paneles anteriores volvió a abrirse. Cancelando.");
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

        Debug.Log("[MostrarPanelDelay] Panel 3 mostrado exitosamente.");
    }
}