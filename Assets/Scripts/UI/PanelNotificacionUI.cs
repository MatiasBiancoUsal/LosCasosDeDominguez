using System.Collections;
using UnityEngine;
using TMPro; // Usar si tenés TextMeshPro, si usás Text común usá UnityEngine.UI

public class PanelNotificacionUI : MonoBehaviour
{
    [Header("Configuración del Panel")]
    [SerializeField] private GameObject contenedorPanel;
    [SerializeField] private float tiempoVisible = 5f;

    [Header("Filtro de Bandera (Opcional)")]
    [Tooltip("Si asignás una bandera, solo se mostrará cuando se obtenga ESTA bandera en particular.")]
    [SerializeField] private GameFlag banderaAAtender;

    private Coroutine rutinaOcultar;

    private void OnEnable()
    {
        // Nos suscribimos al evento del GameStateManager cuando se activa el objeto
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += MostrarNotificacion;
        }
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= MostrarNotificacion;
        }
    }

    private void MostrarNotificacion(GameFlag banderaConseguida)
    {
        // Si especificamos una bandera en particular y no es la que llegó, la ignoramos
        if (banderaAAtender != null && banderaConseguida != banderaAAtender)
            return;

        // Encendemos el panel
        contenedorPanel.SetActive(true);

        // Si ya había una cuenta regresiva corriendo, la reiniciamos
        if (rutinaOcultar != null)
            StopCoroutine(rutinaOcultar);

        rutinaOcultar = StartCoroutine(RutinaOcultarPorTiempo());
    }

    private IEnumerator RutinaOcultarPorTiempo()
    {
        yield return new WaitForSeconds(tiempoVisible);
        CerrarNotificacion();
    }

    /// <summary>
    /// Asignar esta función al evento OnClick() del botón "X" de la UI
    /// </summary>
    public void CerrarNotificacion()
    {
        if (rutinaOcultar != null)
            StopCoroutine(rutinaOcultar);

        contenedorPanel.SetActive(false);
    }
}