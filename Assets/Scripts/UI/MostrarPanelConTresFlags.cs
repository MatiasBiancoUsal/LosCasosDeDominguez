using UnityEngine;
using TMPro;
using System.Collections;

public class MostrarPanelConTresFlags : MonoBehaviour
{
    [Header("Flags que cuentan para la colección")]
    public GameFlag flag1;
    public GameFlag flag2;
    public GameFlag flag3;

    [Header("Panel de notificación (Panel 2 - Contador)")]
    public GameObject panel;

    [Header("Panel que debe cerrarse primero (Inspección)")]
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

        ActualizarContador();

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
        // Esperamos a que se cierre el panel de inspección anterior
        if (esperandoPanel && !esperandoDelay)
        {
            if (panelAnterior == null || !panelAnterior.activeSelf)
            {
                StartCoroutine(MostrarConDelay());
            }
        }

        // Si el cartel del contador está activo y se presiona X, se cierra
        if (panel != null &&
            panel.activeSelf &&
            Input.GetKeyDown(KeyCode.X))
        {
            panel.SetActive(false);
            esperandoPanel = false;
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        if (!EsUnaDeLasTres(bandera))
            return;

        ActualizarContador();

        // Solo activamos la espera para mostrar el cartel si el panel no está ya activo
        if (panel != null && !panel.activeSelf)
        {
            esperandoPanel = true;
        }
    }

    private IEnumerator MostrarConDelay()
    {
        esperandoDelay = true;

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        if (panelAnterior != null && panelAnterior.activeSelf)
        {
            esperandoDelay = false;
            yield break;
        }

        MostrarCartel();

        esperandoDelay = false;
    }

    private bool EsUnaDeLasTres(GameFlag bandera)
    {
        if (bandera == null) return false;
        return bandera == flag1 || bandera == flag2 || bandera == flag3;
    }

    private int ContarFlagsObtenidas()
    {
        int cantidad = 0;
        if (flag1 != null && GameStateManager.Instance.TieneBandera(flag1)) cantidad++;
        if (flag2 != null && GameStateManager.Instance.TieneBandera(flag2)) cantidad++;
        if (flag3 != null && GameStateManager.Instance.TieneBandera(flag3)) cantidad++;
        return cantidad;
    }

    private void ActualizarContador()
    {
        if (contadorTexto == null) return;
        int cantidad = ContarFlagsObtenidas();
        contadorTexto.text = cantidad + "/3";
    }

    private void MostrarCartel()
    {
        if (panel == null) return;
        panel.SetActive(true);
        esperandoPanel = false;
        Debug.Log("[MostrarPanelConTresFlags] Contador mostrado: " + ContarFlagsObtenidas() + "/3");
    }
}