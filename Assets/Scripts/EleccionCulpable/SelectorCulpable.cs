using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorCulpable : MonoBehaviour
{
    // Carga la escena correspondiente al sospechoso
    public void CargarEscenaFinal(string nombreEscenaFinal)
    {
        if (string.IsNullOrEmpty(nombreEscenaFinal))
        {
            Debug.LogWarning("[SelectorCulpable] El nombre de la escena está vacío.");
            return;
        }

        SceneManager.LoadScene(nombreEscenaFinal);
    }
}
