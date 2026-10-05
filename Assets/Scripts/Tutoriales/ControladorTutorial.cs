using UnityEngine;

public class ControladorTutorial : MonoBehaviour
{
    public enum MostrarTuotial { no, [InspectorName("Sí")] si }
    public MostrarTuotial usarTutorial;

    [Header("Paneles del Tutorial")]
    public GameObject panelTutorial1;
    public GameObject panelTutorial2;
    public GameObject panelTutorial3;

    public movimiento _horacio;

    public static bool tutorialActivo;

    private void Start()
    {
        // Solo se abre solo al iniciar la escena si 'usarTutorial' está marcado como 'si'
        if (usarTutorial == MostrarTuotial.si)
        {
            AbrirTutorial();
        }
        else
        {
            OcultarTodosLosPaneles();
        }
    }

    private void Update()
    {
        // Si el tutorial está visible y presionan 'X', se cierra
        if (tutorialActivo && Input.GetKeyDown(KeyCode.X))
        {
            CerrarTutorial();

            if (_horacio != null)
            {
                _horacio.HabilitarCamianata(true);
            }
        }
    }

    // Método para abrir el tutorial (llamado al inicio o desde el Menú de Pausa)
    public void AbrirTutorial()
    {
        gameObject.SetActive(true);
        MostrarPanel(1);
        tutorialActivo = true;
        Time.timeScale = 0f; // Mantiene el juego pausado
    }

    // Maneja la visibilidad de las 3 páginas
    public void MostrarPanel(int numeroPanel)
    {
        if (panelTutorial1 != null) panelTutorial1.SetActive(numeroPanel == 1);
        if (panelTutorial2 != null) panelTutorial2.SetActive(numeroPanel == 2);
        if (panelTutorial3 != null) panelTutorial3.SetActive(numeroPanel == 3);
    }

    // Funciones para conectar directamente en las flechas de los botones (On Click)
    public void IrAPanel1() => MostrarPanel(1);
    public void IrAPanel2() => MostrarPanel(2);
    public void IrAPanel3() => MostrarPanel(3);

    public void CerrarTutorial()
    {
        OcultarTodosLosPaneles();
        tutorialActivo = false;

        // Verificamos si estamos dentro del Menú de Pausa
        MenuPausa menuPausa = FindFirstObjectByType<MenuPausa>();
        if (menuPausa != null && menuPausa.juegoPausado)
        {
            // Volvemos al menú principal de pausa en vez de despausar el juego completo
            menuPausa.VolverAlMenuPausaPrincipal();
        }
        else
        {
            // Si era el tutorial inicial del nivel, reanudamos el juego
            Time.timeScale = 1f;
        }

        gameObject.SetActive(false);
    }

    private void OcultarTodosLosPaneles()
    {
        if (panelTutorial1 != null) panelTutorial1.SetActive(false);
        if (panelTutorial2 != null) panelTutorial2.SetActive(false);
        if (panelTutorial3 != null) panelTutorial3.SetActive(false);
    }
}