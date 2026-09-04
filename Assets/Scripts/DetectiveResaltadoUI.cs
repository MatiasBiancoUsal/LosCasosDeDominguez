using UnityEngine;

public class DetectiveResaltadoUI : MonoBehaviour
{
    [Header("Banderas de la Historia")]
    [SerializeField] private GameFlag banderaRequeridaLlamada;
    [SerializeField] private GameFlag banderaCompletadaDetective;

    [Header("Referencias Visuales")]
    [SerializeField] private GameObject iconoResaltado; // Borde / Brillo
    [SerializeField] private GameObject botonConversarUI; // Cartel 'Presiona I'

    private DetectorHover hover;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (GameStateManager.Instance == null) return;

        // 1. Comprobar si la historia está en el punto exacto para mostrar el borde
        bool llamadaHecha = GameStateManager.Instance.TieneBandera(banderaRequeridaLlamada);
        bool yaHabloConDetective = banderaCompletadaDetective != null &&
                                   GameStateManager.Instance.TieneBandera(banderaCompletadaDetective);

        bool debeEstarActivo = llamadaHecha && !yaHabloConDetective;

        // 2. Controlar el Borde / Brillo (Independiente del mouse)
        if (iconoResaltado != null && iconoResaltado.activeSelf != debeEstarActivo)
        {
            iconoResaltado.SetActive(debeEstarActivo);
        }

        // 3. Controlar el botón "Presiona I" (Requiere estar activo Y tener el mouse encima)
        bool mostrarBoton = debeEstarActivo && (hover != null && hover.MouseEstaEncima);

        if (botonConversarUI != null && botonConversarUI.activeSelf != mostrarBoton)
        {
            botonConversarUI.SetActive(mostrarBoton);
        }
    }
}