using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivadorCinematicas : MonoBehaviour
{
    public Animator anim;
    public Canvas canva;
    public GameObject objetoCinematica; // Nueva variable sin eliminar 'canva'
    public int escenaDestino;

    void Start()
    {

    }

    public void termina()
    {
        // Oculta el Canvas si está asignado
        if (canva != null)
        {
            canva.gameObject.SetActive(false);
        }

        // Oculta el GameObject si está asignado
        if (objetoCinematica != null)
        {
            objetoCinematica.SetActive(false);
        }

        // Carga la escena de destino
        SceneManager.LoadScene(escenaDestino);
    }
}