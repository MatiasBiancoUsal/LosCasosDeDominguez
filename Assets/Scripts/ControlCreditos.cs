using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCreditos : MonoBehaviour
{
    [Header("Referencias UI")]
    public CanvasGroup grupoBotones;
    public float duracionFade = 1.5f;

    [Header("Escenas")]
    public string nombreEscenaMenu = "MenuPrincipal";

    public void MostrarBotonesFinales()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            grupoBotones.alpha = Mathf.Clamp01(tiempo / duracionFade);
            yield return null;
        }

        grupoBotones.alpha = 1f;
        grupoBotones.interactable = true;
        grupoBotones.blocksRaycasts = true;
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void SalirDelJuego()
{
    Debug.Log("Saliendo del juego...");
    
    Application.Quit();
}
}