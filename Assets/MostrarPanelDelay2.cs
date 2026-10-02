using UnityEngine;
using System.Collections;

public class MostrarPanelDelay2 : MonoBehaviour
{
    [Header("Las 3 Flags de Colección")]
    public GameFlag papel1;
    public GameFlag papel2;
    public GameFlag papel3;

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

        // --- SI EL PANEL YA ESTÁ VISIBLE EN PANTALLA ---
        if (panelMostrado)
        {
            // Solo dejamos cerrar este panel si el jugador PRESIONA Y SUELTA o presiona X
            // DESPUÉS de que el panel ya estaba abierto y listo
            if (puedoCerrarConX && Input.GetKeyDown(KeyCode.X))
            {
                panel.SetActive(false);
                panelMostrado = false;
                puedoCerrarConX = false;
                Debug.Log("[MostrarPanelDelay2] Panel 'Unir Papeles' cerrado correctamente con X.");
            }
            return;
        }

        // Si ya se mostró en la partida o está abriéndose, no hacemos nada
        if (PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1 || procesandoApertura) return;

        // --- EVALUACIÓN DE APERTURA ---
        if (TengoLosTresPapeles())
        {
            // 1. Detectamos si el cartel del contador está abierto
            if (panelAnterior2 != null && panelAnterior2.activeSelf)
            {
                contadorSeAbrioAlgunaVez = true;
            }

            // 2. Revisamos si AMBOS paneles ya están cerrados
            bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
            bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

            // SOLO iniciamos si ambos están cerrados Y si el cartel del contador ya estuvo abierto y se cerró
            if (p1Cerrado && p2Cerrado && contadorSeAbrioAlgunaVez)
            {
                StartCoroutine(MostrarSecuencia());
            }
        }
    }

    private IEnumerator MostrarSecuencia()
    {
        procesandoApertura = true;

        // Esperamos a que el jugador SUELTE la tecla X del panel anterior por completo
        while (Input.GetKey(KeyCode.X))
        {
            yield return null;
        }

        // Pequeña pausa extra para fluidez visual
        yield return new WaitForSeconds(delayDespuesDeCerrar);

        // Confirmación final de seguridad
        bool p1Cerrado = (panelAnterior1 == null || !panelAnterior1.activeSelf);
        bool p2Cerrado = (panelAnterior2 == null || !panelAnterior2.activeSelf);

        if (p1Cerrado && p2Cerrado)
        {
            PlayerPrefs.SetInt(ClaveNotificacion, 1);
            PlayerPrefs.Save();

            panel.SetActive(true);
            panelMostrado = true;

            // Damos un margen razonable antes de permitir que la X vuelva a cerrar ESTE panel
            yield return new WaitForSeconds(0.2f);
            puedoCerrarConX = true;

            Debug.Log("[MostrarPanelDelay2] ¡ÉXITO! Cartel 'Unir los papeles' abierto en secuencia correcta.");
        }

        procesandoApertura = false;
    }

    private bool TengoLosTresPapeles()
    {
        if (GameStateManager.Instance == null) return false;

        bool p1 = papel1 != null && GameStateManager.Instance.TieneBandera(papel1);
        bool p2 = papel2 != null && GameStateManager.Instance.TieneBandera(papel2);
        bool p3 = papel3 != null && GameStateManager.Instance.TieneBandera(papel3);

        return p1 && p2 && p3;
    }
}