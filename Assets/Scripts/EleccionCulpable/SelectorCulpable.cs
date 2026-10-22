using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCulpable : MonoBehaviour
{
    [Header("Condiciones de Apertura")]
    [Tooltip("Primera bandera requerida para abrir el panel.")]
    [SerializeField] private GameFlag banderaRequisito1;

    [Tooltip("Segunda bandera requerida para abrir el panel.")]
    [SerializeField] private GameFlag banderaRequisito2;

    [Tooltip("Tercera bandera requerida para abrir el panel.")]
    [SerializeField] private GameFlag banderaRequisito3;

    [Tooltip("Opcional: Bandera para que el panel no vuelva a salir si el caso ya se resolvió.")]
    [SerializeField] private GameFlag banderaCasoResuelto;

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelContenedor;

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += EvaluarAperturaPanel;
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= EvaluarAperturaPanel;
        }
    }

    private void Start()
    {
        ComprobarYMostrarPanel();
    }

    private void EvaluarAperturaPanel(GameFlag banderaRecienGuardada)
    {
        ComprobarYMostrarPanel();
    }

    public void ComprobarYMostrarPanel()
    {
        if (GameStateManager.Instance == null) return;

        if (banderaCasoResuelto != null && GameStateManager.Instance.TieneBandera(banderaCasoResuelto))
        {
            OcultarPanel();
            return;
        }

        if (TieneTodasLasBanderas())
        {
            MostrarPanel();
        }
        else
        {
            OcultarPanel();
        }
    }

    private bool TieneTodasLasBanderas()
    {
        bool cumple1 = banderaRequisito1 != null && GameStateManager.Instance.TieneBandera(banderaRequisito1);
        bool cumple2 = banderaRequisito2 != null && GameStateManager.Instance.TieneBandera(banderaRequisito2);
        bool cumple3 = banderaRequisito3 != null && GameStateManager.Instance.TieneBandera(banderaRequisito3);

        return cumple1 && cumple2 && cumple3;
    }

    private void MostrarPanel()
    {
        if (panelContenedor != null)
            panelContenedor.SetActive(true);
        else
            gameObject.SetActive(true);
    }

    private void OcultarPanel()
    {
        if (panelContenedor != null)
            panelContenedor.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    public void ElegirCulpable(string datoCompleto)
    {
        if (string.IsNullOrEmpty(datoCompleto)) return;

        string nombreSospechoso = datoCompleto;
        string nombreEscenaFinal = datoCompleto;

        if (datoCompleto.Contains("|"))
        {
            string[] partes = datoCompleto.Split('|');
            nombreSospechoso = partes[0];
            nombreEscenaFinal = partes[1];
        }

        print($"[Analytics] Se votó al sospechoso: {nombreSospechoso} (Cargando escena: {nombreEscenaFinal})");

        //DESCOENTARLO PARA QUE ENVIE EL DATO
        if (enviardato.singleton != null)
        {
            enviardato.singleton.eventoelegirculpable(nombresospechoso);
        }

        if (!string.IsNullOrEmpty(nombreEscenaFinal))
        {
            SceneManager.LoadScene(nombreEscenaFinal);
        }
    }
}