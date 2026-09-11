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

    [Header("Contador")]
    public TMP_Text contadorTexto;

    [Header("Tiempo que permanece visible")]
    public float duracion = 3f;

    private Coroutine rutinaPanel;

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

    private void AlObtenerBandera(GameFlag bandera)
    {
        // Si la bandera obtenida NO es una de nuestras 3,
        // ignoramos el evento.
        if (!EsUnaDeLasTres(bandera))
            return;

        // Actualizamos el contador
        ActualizarContador();

        // Mostramos la notificación SIEMPRE que agarre
        // una de estas tres flags.
        MostrarCartel();
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