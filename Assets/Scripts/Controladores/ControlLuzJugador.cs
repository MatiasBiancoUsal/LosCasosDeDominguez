using UnityEngine;

public class ControlLuzJugador : MonoBehaviour
{
    [Header("Configuración de Luz")]
    [Tooltip("El objeto Light 2D hijo del jugador.")]
    [SerializeField] private GameObject luzJugador;

    [Tooltip("La bandera que indica si el jugador ya tiene la lámpara.")]
    [SerializeField] private GameFlag banderaLampara;

    private void Start()
    {
        ComprobarLuz();
    }

    public void ComprobarLuz()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("[ControlLuzJugador] No hay GameStateManager en la escena.");
            return;
        }

        bool tieneLampara = GameStateManager.Instance.TieneBandera(banderaLampara);

        if (luzJugador != null)
        {
            luzJugador.SetActive(tieneLampara);
        }
    }
}