using System.Collections;
using UnityEngine;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject canvasArmarioUI; 
    [SerializeField] private TextMeshProUGUI textoTiempo; 
    [SerializeField] private GameObject botonAbrirArmario; 
    // El botón para activar el minijuego (tendriamos que ponerlo encima del armario, ver con las chicas que letra lo activa)

    [Header("Configuración de Tiempos")]
    [SerializeField] private float tiempoLimite = 12f;   // 12 Segundos para buscar (ir probando si mas o menos segun cuanto tardemos nosotras)
    [SerializeField] private float tiempoCooldown = 30f; // Tiempo de espera si falla (para que no puedas arrancar directamente, sino que tengas que esperar)

    private int pistasEncontradas = 0;
    private float tiempoRestante;
    private bool juegoActivo = false;
    private bool enCooldown = false;
    private Coroutine corrutinaTiempo;

    public void AbrirMinijuegoArmario()
    {
        if (enCooldown)
        {
            Debug.Log("El detective dice: 'Recién revisé ahí, necesito un momento'.");
            return;
        }

        // Resetear variables
        pistasEncontradas = 0;
        tiempoRestante = tiempoLimite;
        juegoActivo = true;

        canvasArmarioUI.SetActive(true);

        if (botonAbrirArmario != null) botonAbrirArmario.SetActive(false);

        corrutinaTiempo = StartCoroutine(Contrarreloj());
    }

    private IEnumerator Contrarreloj()
    {
        while (tiempoRestante > 0 && juegoActivo)
        {
            tiempoRestante -= Time.deltaTime;
            textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString() + "s";
            yield return null;
        }

        if (juegoActivo && pistasEncontradas < 3)
        {
            TiempoAgotado();
        }
    }

    public void PistaEncontrada(GameObject objetoPista)
    {
        if (!juegoActivo) return;

        objetoPista.SetActive(false);
        pistasEncontradas++;

        if (pistasEncontradas >= 3)
        {
            GanarMinijuego();
        }
    }

    private void GanarMinijuego()
    {
        juegoActivo = false;
        if (corrutinaTiempo != null) StopCoroutine(corrutinaTiempo);

        canvasArmarioUI.SetActive(false);
        Debug.Log("¡Pistas encontradas con éxito!");

    }

    private void TiempoAgotado()
    {
        juegoActivo = false;
        canvasArmarioUI.SetActive(false); 

        StartCoroutine(IniciarCooldown());
    }

    private IEnumerator IniciarCooldown()
    {
        enCooldown = true;
        if (botonAbrirArmario != null) botonAbrirArmario.SetActive(false); 

        yield return new WaitForSeconds(tiempoCooldown); 

        enCooldown = false;
        if (botonAbrirArmario != null) botonAbrirArmario.SetActive(true); 
    }
}