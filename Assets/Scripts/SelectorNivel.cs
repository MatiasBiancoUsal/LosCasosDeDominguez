using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNivel : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Escribe aquí el nombre exacto de la escena a la que lleva este objeto")]
    public string nombreDeLaEscena;

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
        CargarNivelBoton();
    }
}