using UnityEngine;
using TMPro;
using System.Collections;

public class MostrarPanelConTresFlags : MonoBehaviour
{
    [Header("Flags que cuentan para la colección")]
    public GameFlag flag1;
    public GameFlag flag2;
    public GameFlag flag3;

    [Header("Panel de notificación")]
    public GameObject panel;

    [Header("Panel que debe cerrarse primero")]
    public GameObject panelAnterior;

    [Header("Delay después de cerrar el panel anterior")]
    [SerializeField] private float delayDespuesDeCerrar = 0.2f;

    [Header("Contador")]
    public TMP_Text contadorTexto;

    private bool esperandoPanel = false;
    private bool esperandoDelay = false;

    private void Start()
    {
        if (GameStateManager.Instance == null)
            return;

        // Actualiza el contador apenas empieza la escena
        ActualizarContador();

        // Escuchamos cada vez que se obtiene una flag
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
        if (esperandoPanel && !esperandoDelay)
        {
            if (panelAnterior == null || !panelAnterior.activeSelf)
            {
                StartCoroutine(MostrarConDelay());
            }
        }

        // Si el panel está activo y se presiona X, lo cerramos
        if (panel != null &&
            panel.activeSelf &&
            Input.GetKeyDown(KeyCode.X))
        {
            panel.SetActive(false);
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        // Si la bandera obtenida NO es una de nuestras 3,
        // ignoramos el evento.
        if (!EsUnaDeLasTres(bandera))
            return;

        // Actualizamos el contador
        ActualizarContador();

        // Esperamos a que se cierre el panel anterior
        esperandoPanel = true;
    }

    private IEnumerator MostrarConDelay()
    {
        esperandoDelay = true;

        Debug.Log("[MostrarPanelConTresFlags] Panel anterior cerrado. Esperando "
                  + delayDespuesDeCerrar + " segundos...");

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Volvemos a comprobar por seguridad
        if (panelAnterior != null && panelAnterior.activeSelf)
        {
            Debug.Log("[MostrarPanelConTresFlags] El panel anterior volvió a abrirse. Cancelando.");

            esperandoDelay = false;
            yield break;
        }

        MostrarCartel();

        esperandoPanel = false;
        esperandoDelay = false;
    }

    private bool EsUnaDeLasTres(GameFlag bandera)
    {
        if (bandera == null)
            return false;

        return bandera == flag1 ||
               bandera == flag2 ||
               bandera == flag3;
    }

    private int ContarFlagsObtenidas()
    {
        int cantidad = 0;

        if (flag1 != null &&
            GameStateManager.Instance.TieneBandera(flag1))
        {
            cantidad++;
        }

        if (flag2 != null &&
            GameStateManager.Instance.TieneBandera(flag2))
        {
            cantidad++;
        }

        if (flag3 != null &&
            GameStateManager.Instance.TieneBandera(flag3))
        {
            cantidad++;
        }

        return cantidad;
    }

    private void ActualizarContador()
    {
        if (contadorTexto == null)
            return;

        int cantidad = ContarFlagsObtenidas();

        contadorTexto.text = cantidad + "/3";
    }

    private void MostrarCartel()
    {
        if (panel == null)
            return;

        panel.SetActive(true);

        Debug.Log("[MostrarPanelConTresFlags] Nuevo panel mostrado.");
    }
}