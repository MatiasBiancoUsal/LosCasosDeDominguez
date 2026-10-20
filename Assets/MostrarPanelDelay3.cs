using UnityEngine;
using System.Collections;

public class MostrarPanelDelay3 : MonoBehaviour
{
    [Header("Flag necesaria")]
    public GameFlag papel;

    [Header("Panel a mostrar (Este Panel / Panel 3)")]
    public GameObject panel;

    [Header("Paneles que deben estar cerrados primero")]
    public GameObject panelAnterior1; // PanelObjetos
    public GameObject panelAnterior2; // Encontraste un papel...

    [Header("Delay después de que se cierren ambos paneles")]
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

                Debug.Log("[MostrarPanelDelay2] Panel cerrado correctamente con X.");
            }

            return;
        }

        // Si ya se mostró o está abriéndose, no hacemos nada
        if (PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1 || procesandoApertura)
            return;

        // Evaluamos si tenemos la flag necesaria
        if (TengoElPapel())
        {
            // Detectamos si el contador estuvo abierto
            if (panelAnterior2 != null && panelAnterior2.activeSelf)
            {
                contadorSeAbrioAlgunaVez = true;
            }

            // Comprobamos que ambos paneles estén cerrados
            bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
            bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

            // Abrimos únicamente cuando se cumplen todas las condiciones
            if (p1Cerrado && p2Cerrado && contadorSeAbrioAlgunaVez)
            {
                StartCoroutine(MostrarSecuencia());
            }
        }
    }

    private IEnumerator MostrarSecuencia()
    {
        procesandoApertura = true;

        // Esperamos a que el jugador suelte la tecla X
        while (Input.GetKey(KeyCode.X))
        {
            yield return null;
        }

        // Pausa extra para evitar conflictos visuales
        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Confirmación final
        bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
        bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

        if (p1Cerrado && p2Cerrado)
        {
            PlayerPrefs.SetInt(ClaveNotificacion, 1);
            PlayerPrefs.Save();

            panel.SetActive(true);
            panelMostrado = true;

            // Evitamos que la misma X cierre el nuevo panel inmediatamente
            yield return new WaitForSeconds(0.2f);
            puedoCerrarConX = true;

            Debug.Log("[MostrarPanelDelay2] Panel abierto correctamente.");
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