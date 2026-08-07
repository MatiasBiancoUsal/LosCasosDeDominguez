using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCinematica2 : MonoBehaviour
{

    public string siguienteEscena = "Recibidor_Nivel2";

    public void TerminarCinematica2()
    {
        SceneManager.LoadScene(siguienteEscena);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            TerminarCinematica2();
        }
    }
}