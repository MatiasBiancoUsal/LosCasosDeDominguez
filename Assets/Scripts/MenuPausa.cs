using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [Header("Paneles de UI")]
    public GameObject panelPausaPrincipal;
    public GameObject panelInstrucciones;
    public GameObject panelSonidos;

    private bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                bool enSubmenu = false;

                if (panelInstrucciones != null && panelInstrucciones.activeSelf) enSubmenu = true;
                if (panelSonidos != null && panelSonidos.activeSelf) enSubmenu = true;

                if (enSubmenu)
                {
                    VolverAlMenuPausaPrincipal();
                }
                else
                {
                    ContinuarJuego();
                }
            }
            else
            {
                PausarJuego();
            }
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
}