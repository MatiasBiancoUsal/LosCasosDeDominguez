using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    [Header("Paneles de UI")]
    public GameObject panelPausaPrincipal;
    public GameObject panelInstrucciones;
    public GameObject panelSonidos;

    private bool juegoPausado = false;
    public GameObject objetoSonidoActivo;
    public GameObject objetoSonidoDesactivado;

    private bool sonidoActivo = true;


    private void Start()
    {
        Time.timeScale = 1f; 
        juegoPausado = false;

        // Por si el panel de pausa inicia abierto por error en el inspector (pasaba en oficina de Horacio)
        if (panelPausaPrincipal != null)
        {
            panelPausaPrincipal.SetActive(false);
        }
    }

    private void Update()
    {
        Debug.Log("juegoPausado = " + juegoPausado);
        // La P solo abre el menú de pausa
        if (Input.GetKeyDown(KeyCode.P) && !juegoPausado) 
        { 
            PausarJuego(); 
        }
    }


    public void PausarJuego()
    {
        panelPausaPrincipal.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void ContinuarJuego()
    {
        Debug.Log("CONTINUAR JUEGO");

        panelPausaPrincipal.SetActive(false);

        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (panelSonidos != null) panelSonidos.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void VolverAlMenuPausaPrincipal()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (panelSonidos != null) panelSonidos.SetActive(false);


        panelPausaPrincipal.SetActive(true);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void AbrirInstrucciones()
    {
        panelPausaPrincipal.SetActive(false);
        if (panelInstrucciones != null) panelInstrucciones.SetActive(true);
    }

    public void AbrirSonidos()
    {
        panelPausaPrincipal.SetActive(false);
        if (panelSonidos != null) panelSonidos.SetActive(true);
    }

    public void AlternarPausaBoton()
    {
        if (Time.timeScale == 0f)
        {
            ContinuarJuego();
        }

        else
        {
            PausarJuego();
        }
    }

    public void AlternarSonido()
    {
        sonidoActivo = !sonidoActivo;

        if (sonidoActivo)
        {
            objetoSonidoActivo.SetActive(true);
            objetoSonidoDesactivado.SetActive(false);
            AudioListener.volume = 1f;
            Debug.Log("Sonido Activado");
        }
        else
        {
            objetoSonidoActivo.SetActive(false);
            objetoSonidoDesactivado.SetActive(true);
            AudioListener.volume = 0f;
            Debug.Log("Sonido Desactivado");
        }
    }

    public void DespausarPostTutorial()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        Debug.Log("Tutorial cerrado: Tiempo reanudado");
    }
}