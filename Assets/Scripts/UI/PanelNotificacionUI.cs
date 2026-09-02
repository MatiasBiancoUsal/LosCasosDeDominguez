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

    private void Start()
    {
        // Por si la flag ya había sido obtenida anteriormente
        // antes de cargar esta escena.
        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.TieneBandera(flagNecesaria))
        {
            MostrarCartel();
        }

        // Escuchamos cuando se obtiene una nueva bandera.
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += AlObtenerBandera;
        }
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
        if (bandera == flagNecesaria)
        {
            MostrarCartel();
        }
    }

    private void MostrarCartel()
    {
        if (panel == null) return;

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