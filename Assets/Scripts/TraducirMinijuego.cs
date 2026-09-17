using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TraducirMinijuego : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject papelUnido;
    public GameObject botonTraducir;
    public TextMeshProUGUI textoMensaje;
    public string nombreEscenaHabitacion;

    [Header("Configuración de Flags")]
    [SerializeField] private GameFlag flagPuzzleResuelto; // Asigna aquí el ScriptableObject desde el Inspector

    [Header("Configuración del Mensaje")]
    [TextArea(3, 5)]
    public string mensajeFinal = "Aquí escribes el mensaje secreto que el jugador va a descubrir...";
    public float velocidadEscritura = 0.05f;

    private int pedazosArmados = 0;

    void Start()
    {
        textoMensaje.text = "";
    }

    public void TocarPapelRoto(GameObject papelTocado)
    {
        papelTocado.SetActive(false);
        pedazosArmados++;

        if (pedazosArmados >= 3)
        {
            papelUnido.SetActive(true);
            botonTraducir.SetActive(true);
        }
    }

    // Se ejecuta al presionar el botón "TRADUCIR"
    public void EmpezarTraduccion()
    {
        botonTraducir.SetActive(false);

        // Guarda la flag en PlayerPrefs usando el ID del ScriptableObject
        GuardarFlag();

        StartCoroutine(EfectoMaquinaDeEscribir());
    }

    private void GuardarFlag()
    {
        if (flagPuzzleResuelto != null)
        {
            PlayerPrefs.SetInt(flagPuzzleResuelto.Id, 1);
            PlayerPrefs.Save();
            Debug.Log($"Flag '{flagPuzzleResuelto.Id}' guardada con éxito en PlayerPrefs.");
        }
        else
        {
            Debug.LogWarning($"[TraducirMinijuego] No se ha asignado ningún GameFlag en {gameObject.name}.");
        }
    }

    IEnumerator EfectoMaquinaDeEscribir()
    {
        textoMensaje.text = "";

        foreach (char letra in mensajeFinal.ToCharArray())
        {
            textoMensaje.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        Debug.Log("Traducción terminada!");
    }

    public void VolverAHabitacion()
    {
        ArmarioInteractuable.ActivarCooldown(10f);
        SceneManager.LoadScene(nombreEscenaHabitacion);
    }

    public void VolverAlJuego()
    {
        SceneManager.LoadScene("NombreDeTuEscenaPrincipal");
    }
}