using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorEscenaOscura : MonoBehaviour
{
    [Header("Banderas y Estado")]
    [SerializeField] private GameFlag banderaLampara;

    [Header("Interfaz de Usuario (Cartel)")]
    [SerializeField] private GameObject cartelSinLampara;

    [Header("Configuración de Retorno")]
    [SerializeField] private bool regresoAutomatico = true;
    [SerializeField] private float tiempoParaVolver = 3.0f;
    [SerializeField] private string escenaAnterior = "GranSalon_Nivel3";

    private bool faltaLampara = false;

    private void Start()
    {
        // Esperamos 1 frame para garantizar que GameStateManager ya arrancó
        StartCoroutine(VerificarConRetraso());
    }

    private IEnumerator VerificarConRetraso()
    {
        yield return null; // Espera un frame frame de renderizado

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
            Debug.LogError("[ControladorEscenaOscura] No se encontró el GameStateManager en la escena.");
            return;
        }

        if (banderaLampara == null)
        {
            Debug.LogError("[ControladorEscenaOscura] Falta asignar el asset 'Lampara' en el Inspector.");
            return;
        }

        bool tieneLampara = GameStateManager.Instance.TieneBandera(banderaLampara);
        Debug.Log($"[ControladorEscenaOscura] ¿Tiene la lámpara?: {tieneLampara}");

        if (!tieneLampara)
        {
            faltaLampara = true;

            if (cartelSinLampara != null)
            {
                cartelSinLampara.SetActive(true);
                Debug.Log("[ControladorEscenaOscura] ¡Cartel Activado!");
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