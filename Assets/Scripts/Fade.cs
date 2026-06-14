using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Fade : MonoBehaviour
{
    public Animator animator;
    public float tiempoDeEspera = 1f; 

    public void CambiarEscenaConFade(string nombreEscena)
    {
        StartCoroutine(Transicion(nombreEscena));
    }

    IEnumerator Transicion(string nombreEscena)
    {
        animator.Play("FadeOut");

        yield return new WaitForSeconds(tiempoDeEspera);

        SceneManager.LoadScene(nombreEscena);
    }
}
