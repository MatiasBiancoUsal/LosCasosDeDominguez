using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCulpable : MonoBehaviour
{
    [Header("Condiciones de Activación")]
    [Tooltip("Las 3 banderas de los personajes con los que hay que hablar antes de activar este panel")]
    [SerializeField] private GameFlag[] banderasRequeridas;

    [Tooltip("Bandera opcional por si el caso ya fue resuelto previamente y no debe volver a mostrarse")]
    [SerializeField] private GameFlag banderaCasoResuelto;

    [Header("Referencias UI")]
    [Tooltip("El GameObject del panel contenedor. Si este script está adjunto al mismo Panel, podés dejarlo nulo o asignarlo.")]
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

    private void ComprobarYMostrarPanel()
    {
        if (GameStateManager.Instance == null) return;

        if (banderaCasoResuelto != null && GameStateManager.Instance.TieneBandera(banderaCasoResuelto))
        {
            OcultarPanel();
            return;
        }

        bool tieneTodasLasBanderas = CumpleTodasLasBanderas();

        if (tieneTodasLasBanderas)
        {
            MostrarPanel();
        }
        else
        {
            OcultarPanel();
        }
    }

    private bool CumpleTodasLasBanderas()
    {
        if (banderasRequeridas == null || banderasRequeridas.Length == 0) return false;

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag == null || !GameStateManager.Instance.TieneBandera(flag))
            {
                return false;
            }
        }

        return true;
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
        SceneManager.LoadScene(nombreEscenaFinal);
    }
}