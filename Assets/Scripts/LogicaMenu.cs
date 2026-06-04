using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicaMenu : MonoBehaviour
{

    public GameObject panelMenuPrincipal;
    public GameObject panelOpciones;

    public TextMeshProUGUI textoBotonSonido;
    private bool sonidoActivo = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelMenuPrincipal.SetActive(true);
        panelOpciones.SetActive(false);

    }
    
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    public void Continuar()
    {
        SceneManager.LoadScene(1);
    }

    public void IrAOpciones()
    {
        panelMenuPrincipal.SetActive(false );
        panelOpciones.SetActive(true);
    }
    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    public void VovlerAlMenu()
    {
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    public void AlternarSonido()
    {
        sonidoActivo = !sonidoActivo;

        if (sonidoActivo)
        {
            AudioListener.volume = 1f; // Sonido activado
            textoBotonSonido.text = "Sonido: ON";
        }
        else
        {
            AudioListener.volume = 0f; // Sonido muteado
            textoBotonSonido.text = "Sonido: OFF";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
