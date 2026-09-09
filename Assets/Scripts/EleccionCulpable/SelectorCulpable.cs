using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCulpable : MonoBehaviour
{
    [Header("Condición de Apertura")]
    [Tooltip("Asigná aquí la bandera que otorga el Detective al terminar su diálogo final (ej: HabloConDetectiveFinal).")]
    [SerializeField] private GameFlag banderaDetectiveFinal;

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

        // Se abre SOLO si ya se obtuvo la bandera del Detective
        if (banderaDetectiveFinal != null && GameStateManager.Instance.TieneBandera(banderaDetectiveFinal))
        {
            MostrarPanel();
        }
        else
        {
            OcultarPanel();
        }
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