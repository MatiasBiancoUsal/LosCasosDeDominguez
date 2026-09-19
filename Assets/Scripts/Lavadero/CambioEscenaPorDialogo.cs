using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaPorDialogo : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [SerializeField] private string nombreEscenaDestino;

    [Header("Condición de Activación")]
    [SerializeField] private GameFlag banderaFinalDialogo;

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida += VerificarCambioDeEscena;
            Debug.Log($"[CambioEscena] Suscrito con éxito al evento en {gameObject.name}");
        }
        else
        {
            Debug.LogError($"[CambioEscena] GameStateManager.Instance es NULL en {gameObject.name}");
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= VerificarCambioDeEscena;
        }
    }

    private void VerificarCambioDeEscena(GameFlag banderaRecienObtenida)
    {
        Debug.Log($"[CambioEscena] Bandera recibida: {banderaRecienObtenida?.name} | Buscada: {banderaFinalDialogo?.name}");

        if (banderaFinalDialogo != null && banderaRecienObtenida == banderaFinalDialogo)
        {
            Debug.Log($"[CambioEscena] ¡Coincidencia! Cargando escena: {nombreEscenaDestino}");
            CargarEscena();
        }
    }

    public void CargarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogError($"[CambioEscena] Falta definir 'nombreEscenaDestino' en {gameObject.name}");
        }
    }
}