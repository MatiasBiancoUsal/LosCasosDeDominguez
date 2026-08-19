using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class ControladorEscenaOscura : MonoBehaviour
{
    [Header("Banderas y Estado")]
    [SerializeField] private GameFlag banderaLampara;

    [Header("Interfaz de Usuario (Cartel)")]
    [SerializeField] private GameObject cartelSinLampara;

    [Header("Configuración de Retorno")]
    [SerializeField] private bool regresoAutomatico = false;
    [SerializeField] private float tiempoParaVolver = 3.0f;
    [SerializeField] private string escenaAnterior = "EscenaPrevia";

    private bool faltaLampara = false;

    private void Start()
    {
        ComprobarEstadoEscena();
    }

    private void Update()
    {
        if (faltaLampara && !regresoAutomatico)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Return))
            {
                VolverAEscenaAnterior();
            }
        }
    }

    private void ComprobarEstadoEscena()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("[ControladorEscenaOscura] No hay GameStateManager.");
            return;
        }

        if (!GameStateManager.Instance.TieneBandera(banderaLampara))
        {
            faltaLampara = true;

            if (cartelSinLampara != null)
            {
                cartelSinLampara.SetActive(true);
            }

            if (regresoAutomatico)
            {
                Invoke(nameof(VolverAEscenaAnterior), tiempoParaVolver);
            }
        }
        else
        {
            if (cartelSinLampara != null)
            {
                cartelSinLampara.SetActive(false);
            }
        }
    }

    public void VolverAEscenaAnterior()
    {
        SceneManager.LoadScene(escenaAnterior);
    }
}