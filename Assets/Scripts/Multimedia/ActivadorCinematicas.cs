using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivadorCinematicas : MonoBehaviour
{
    
    public Animator anim;
    public Canvas canva;

    void Start()
    {
        
    }
    public void termina()
    {
        SceneManager.LoadScene(2);
    }
}