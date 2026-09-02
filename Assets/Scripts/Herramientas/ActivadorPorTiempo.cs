using UnityEngine;
using UnityEngine.Events;

public class ActivadorPorTiempo : MonoBehaviour
{
    public UnityEvent accion;
    public float _timer;

    [Header("Flag")]
    public GameFlag flagAlActivar;

    private void Start()
    {
        Invoke(nameof(ActivarAccion), _timer);
    }

    void ActivarAccion()
    {
        // Ejecuta la acción configurada
        accion.Invoke();

        // Guarda la flag al mismo tiempo
        if (flagAlActivar != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(flagAlActivar);

            Debug.Log("Flag otorgada por ActivadorPorTiempo: " + flagAlActivar.name);
        }
    }
}