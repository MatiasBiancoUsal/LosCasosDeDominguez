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

        // Solo permite usar Q si el mouse está sobre este objeto
        if (Keyboard.current.qKey.wasPressedThisFrame && estaSobreElObjeto)
        {
            CargarEscena();
        }
    }

    private bool estaSobreElObjeto = false;

    private void OnMouseEnter()
    {
        estaSobreElObjeto = true;
    }

    private void OnMouseExit()
    {
        estaSobreElObjeto = false;
    }

    private void CargarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}