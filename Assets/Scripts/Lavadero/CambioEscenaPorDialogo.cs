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
        // Si la bandera que se acaba de obtener es la que marca el fin del diálogo del detective
        if (banderaFinalDialogo != null && banderaRecienObtenida == banderaFinalDialogo)
        {
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
            Debug.LogError($"[CambioEscenaPorDialogo] No asignaste la escena de destino en {gameObject.name}");
        }
    }
}