using UnityEngine;

public class DesactivadorPorTiempo : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    [Tooltip("Objeto a ocultar. Si se deja vacío, desactiva este mismo cartel de notificación.")]
    [SerializeField] private GameObject target;

    [Tooltip("Tiempo en segundos que se mantendrá visible la notificación.")]
    [SerializeField] private float tiempo = 5f;

    [Header("Persistencia de Misión (Opcional)")]
    [Tooltip("Si mostrar esta notificación le otorga al jugador una nueva Bandera/Misión, asignala aquí.")]
    [SerializeField] private GameFlag banderaDeMision;

    private void OnEnable()
    {
        // 1. Si la notificación activa una nueva misión/bandera, la guardamos en PlayerPrefs
        if (banderaDeMision != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(banderaDeMision);
        }

        // 2. Reiniciamos la cuenta regresiva por si la notificación se activa varias veces
        CancelInvoke(nameof(Desactivar));
        Invoke(nameof(Desactivar), tiempo);
    }

    private void Desactivar()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}