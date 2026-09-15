using UnityEngine;
using TMPro; // Para el texto
using System.Collections; // Para la corrutina (el efecto de tiempo)
using UnityEngine.SceneManagement; // Para volver a tu juego

public class TraducirMinijuego : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject papelUnido;
    public GameObject botonTraducir;
    public TextMeshProUGUI textoMensaje;

    [Header("Configuración del Mensaje")]
    [TextArea(3, 5)] // Hace que la caja de texto en Unity sea más grande
    public string mensajeFinal = "Aquí escribes el mensaje secreto que el jugador va a descubrir...";
    public float velocidadEscritura = 0.05f; // Tiempo entre cada letra

    private int pedazosArmados = 0;

    void Start()
    {
        // Nos aseguramos de que el texto empiece vacío
        textoMensaje.text = "";
    }

    // Esta función se va a llamar cada vez que toques un papel roto
    public void TocarPapelRoto(GameObject papelTocado)
    {
        // 1. Ocultamos el papel suelto que acabamos de tocar
        papelTocado.SetActive(false);

        // 2. Sumamos 1 al contador
        pedazosArmados++;

        // 3. Si ya tocamos los 3...
        if (pedazosArmados >= 3)
        {
            papelUnido.SetActive(true);   // Mostramos el papel entero
            botonTraducir.SetActive(true); // Aparece el botón
        }
    }

    // Esta función va en el botón "TRADUCIR"
    public void EmpezarTraduccion()
    {
        botonTraducir.SetActive(false); // Ocultamos el botón para que no lo toque de nuevo
        StartCoroutine(EfectoMaquinaDeEscribir()); // Iniciamos el efecto
    }

    // Corrutina que hace la magia de la máquina de escribir
    IEnumerator EfectoMaquinaDeEscribir()
    {
        textoMensaje.text = ""; // Limpiamos por las dudas

        // Recorremos el mensaje letra por letra
        foreach (char letra in mensajeFinal.ToCharArray())
        {
            textoMensaje.text += letra; // Agregamos una letra
            yield return new WaitForSeconds(velocidadEscritura); // Esperamos un poquito
        }

        // Acá el texto ya terminó de escribirse. 
        // Podrías activar un botón de "Volver" o guardar una flag.
        Debug.Log("Traducción terminada!");
        PlayerPrefs.SetInt("PuzzleResuelto", 1); // Guardamos que ya lo resolvió
    }

    // Función para un botón de salir (opcional)
    public void VolverAlJuego()
    {
        SceneManager.LoadScene("NombreDeTuEscenaPrincipal");
    }
}