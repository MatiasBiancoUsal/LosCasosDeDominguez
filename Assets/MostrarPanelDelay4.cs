
using UnityEngine;
using System.Collections;

public class MostrarPanelDelay4 : MonoBehaviour
{
    [Header("Flag necesaria para activar")]
    public GameFlag papel;

    [Header("Panel a mostrar (Este Panel / Panel 3)")]
    public GameObject panel;

    [Header("Paneles que deben estar cerrados primero")]
    public GameObject panelAnterior1;
    public GameObject panelAnterior2;
    public GameObject panelAnterior3;

    [Header("Delay después de que se cierren los tres paneles")]
    [SerializeField] private float delayDespuesDeCerrar = 0.3f;

    private bool contadorSeAbrioAlgunaVez = false;
    private bool procesandoApertura = false;
    private bool panelMostrado = false;
    private bool puedoCerrarConX = false;

    private string ClaveNotificacion => "MostrarPanelDelay_TresPapelesUnir";

    private void Start()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteKey(ClaveNotificacion);
#endif
    }

    private void Update()
    {
        if (panel == null) return;

        // Si el panel ya está visible
        if (panelMostrado)
        {
            if (puedoCerrarConX && Input.GetKeyDown(KeyCode.X))
            {
                panel.SetActive(false);
                panelMostrado = false;
                puedoCerrarConX = false;

                Debug.Log("[MostrarPanelDelay2] Panel cerrado con X.");
            }

            return;
        }

        // Si ya se mostró o está abriéndose, no hacemos nada
        if (PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1 || procesandoApertura)
            return;

        // Evaluamos si el jugador tiene la flag
        if (TengoElPapel())
        {
            // Detectamos si el segundo panel estuvo abierto
            if (panelAnterior2 != null && panelAnterior2.activeSelf)
            {
                contadorSeAbrioAlgunaVez = true;
            }

            // Comprobamos que los tres paneles estén cerrados
            bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
            bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);
            bool p3Cerrado = (panelAnterior3 == null || !panelAnterior3.activeSelf);

            if (p1Cerrado && p2Cerrado && p3Cerrado && contadorSeAbrioAlgunaVez)
            {
                StartCoroutine(MostrarSecuencia());
            }
        }
    }

    private IEnumerator MostrarSecuencia()
    {
        procesandoApertura = true;

        // Esperamos a que se suelte la tecla X
        while (Input.GetKey(KeyCode.X))
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Verificación final
        bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
        bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);
        bool p3Cerrado = (panelAnterior3 == null || !panelAnterior3.activeSelf);

        if (p1Cerrado && p2Cerrado && p3Cerrado)
        {
            PlayerPrefs.SetInt(ClaveNotificacion, 1);
            PlayerPrefs.Save();

            panel.SetActive(true);
            panelMostrado = true;

            // Evitamos que la misma X cierre inmediatamente este panel
            yield return new WaitForSeconds(0.2f);
            puedoCerrarConX = true;

            Debug.Log("[MostrarPanelDelay2] ¡ÉXITO! Panel abierto correctamente.");

            // --- ANALYTICS ---
            string nombreMision = papel != null ? papel.name : panel.name;
            print($"[Analytics] Nueva Misión -> NombreMision: {nombreMision}");

            // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (Lu)
            // EnviarDato.Singleton.EventoNuevaMision(nombreMision);
        }

        procesandoApertura = false;
    }

    private bool TengoElPapel()
    {
        if (GameStateManager.Instance == null) return false;

        return papel != null &&
               GameStateManager.Instance.TieneBandera(papel);
    }
}
