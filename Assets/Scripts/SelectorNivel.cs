using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNivel : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Escribe aquí el nombre exacto de la escena a la que lleva este objeto")]
    public string nombreDeLaEscena;

    [Header("Estado de la puerta")]
    public bool desbloqueado = false;

    public void CargarNivelBoton()
    {
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            SceneManager.LoadScene(nombreDeLaEscena);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Click en " + gameObject.name);

        if (desbloqueado)
        {
            CargarNivelBoton();
        }
        else
        {
            Debug.Log("Esta puerta está bloqueada.");
        }
    }
}