
using UnityEngine;
using System.Collections;

public class MostrarPanelDelay4 : MonoBehaviour
{
    [Header("Flag que activa el panel")]
    public GameFlag flagNecesaria;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Paneles que deben cerrarse primero")]
    public GameObject panelAnterior1;
    public GameObject panelAnterior2;
    public GameObject panelAnterior3;

    [Header("Delay después de cerrar los tres paneles")]
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
        // esperamos a que se cierren los tres paneles,
        // siempre que la notificación no se haya mostrado.
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
        // Esperamos a que los tres paneles estén cerrados
        if (esperandoPanel && !panelMostrado && !esperandoDelay)
        {
            bool p1Cerrado = panelAnterior1 == null || !panelAnterior1.activeSelf;
            bool p2Cerrado = panelAnterior2 == null || !panelAnterior2.activeSelf;
            bool p3Cerrado = panelAnterior3 == null || !panelAnterior3.activeSelf;

            if (p1Cerrado && p2Cerrado && p3Cerrado)
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

        Debug.Log("[MostrarPanelDelay] Los tres paneles anteriores están cerrados. Esperando " +
                  delayDespuesDeCerrar + " segundos...");

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Volvemos a comprobar que los tres paneles sigan cerrados
        bool p1Cerrado = panelAnterior1 == null || !panelAnterior1.activeSelf;
        bool p2Cerrado = panelAnterior2 == null || !panelAnterior2.activeSelf;
        bool p3Cerrado = panelAnterior3 == null || !panelAnterior3.activeSelf;

        if (!p1Cerrado || !p2Cerrado || !p3Cerrado)
        {
            Debug.Log("[MostrarPanelDelay] Un panel anterior volvió a abrirse. Cancelando.");
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

        // --- ANALYTICS ---
        string nombreMision = flagNecesaria != null
            ? flagNecesaria.name
            : panel.name;

        print($"[Analytics] Nueva Misión -> NombreMision: {nombreMision}");

        // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (Lu)
        EnviarDato.Singleton.EventoNuevaMision(nombreMision);
    }
}
