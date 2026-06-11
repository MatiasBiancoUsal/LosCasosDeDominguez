using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivadorCinematicas : MonoBehaviour
{
    public Animator anim;
    public Canvas canva;
    public int escenaDestino;

    void Start()
    {

    }

    public void termina()
    {
        SceneManager.LoadScene(escenaDestino);
    }
}