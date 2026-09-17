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

        // Si ya se resolvió el caso, se oculta
        if (banderaCasoResuelto != null && GameStateManager.Instance.TieneBandera(banderaCasoResuelto))
        {
            OcultarPanel();
            return;
        }

        // Se abre SOLO si se han obtenido las 3 banderas
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
        // Comprueba que las 3 estén asignadas en el Inspector y obtenidas en la partida
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

    public void CargarEscenaFinal(string nombreEscenaFinal)
    {
        if (!string.IsNullOrEmpty(nombreEscenaFinal))
        {
            SceneManager.LoadScene(nombreEscenaFinal);
        }
    }
}