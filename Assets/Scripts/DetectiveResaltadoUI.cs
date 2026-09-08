using UnityEngine;

public class DetectiveResaltadoUI : MonoBehaviour
{
    [Header("Banderas de la Historia")]
    [Tooltip("Todas estas banderas deben estar activas")]
    [SerializeField] private GameFlag[] banderasRequeridas;
    [SerializeField] private GameFlag banderaCompletadaDetective;

    [Header("Referencias Visuales")]
    [SerializeField] private GameObject iconoResaltado; 
    [SerializeField] private GameObject botonConversarUI; 

    private DetectorHover hover;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (GameStateManager.Instance == null) return;

        bool condicionRequeridaCumplida = CumpleTodasLasBanderas();

        bool yaHabloConDetective = banderaCompletadaDetective != null &&
                                   GameStateManager.Instance.TieneBandera(banderaCompletadaDetective);

        bool debeEstarActivo = condicionRequeridaCumplida && !yaHabloConDetective;

        if (iconoResaltado != null && iconoResaltado.activeSelf != debeEstarActivo)
        {
            iconoResaltado.SetActive(debeEstarActivo);
        }

        bool mostrarBoton = debeEstarActivo && (hover != null && hover.MouseEstaEncima);

        if (botonConversarUI != null && botonConversarUI.activeSelf != mostrarBoton)
        {
            botonConversarUI.SetActive(mostrarBoton);
        }
    }

    private bool CumpleTodasLasBanderas()
    {
        if (banderasRequeridas == null || banderasRequeridas.Length == 0) return true;

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag != null && !GameStateManager.Instance.TieneBandera(flag))
            {
                return false; 
            }
        }

        return true; 
    }
}