using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicaMenu : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelOpciones;

    public TextMeshProUGUI textoBotonSonido;
    private bool sonidoActivo = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created void Start()

    void Start()
    {
        panelMenuPrincipal.SetActive(true);
        panelOpciones.SetActive(false);
    }



    public void Jugar()
    {
        
        GameStateManager.Instance.ResetPlayerPrefs();
        SceneManager.LoadScene(1);

    }

    public void Continuar()
    {
        if (!GameStateManager.Instance.HayPartidaGuardada())
        {
            Debug.Log("No hay ninguna partida guardada.");
            return;
        }

        string ultimaEscena = GameStateManager.Instance.ObtenerUltimaEscena();

        Debug.Log("Continuando partida desde: " + ultimaEscena);
        SceneManager.LoadScene(ultimaEscena);
    }

    public void IrAOpciones()
    {
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }


    public void VerCreditos()
    {
        SceneManager.LoadScene("Creditoscinematica");
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
            AudioListener.volume = 1f;
            textoBotonSonido.text = "Sonido: ON";
        }
        else
        {
            AudioListener.volume = 0f;
            textoBotonSonido.text = "Sonido: OFF";
        }
    }

    public void Sonar(AudioClip clip)
    {
        SonidosDeUI.instance.Play(clip);
    }

    // Update is called once per frame void Update()
    void Update()
    {

    }
}