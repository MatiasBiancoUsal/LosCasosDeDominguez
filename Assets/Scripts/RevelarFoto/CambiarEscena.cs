using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreEscenaDestino;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            CargarEscena();
        }
    }

    private void CargarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}