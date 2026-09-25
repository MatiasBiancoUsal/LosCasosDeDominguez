using UnityEngine;

public class ControladorTutorial : MonoBehaviour
{
    public enum MostrarTuotial { no, [InspectorName("Sí")] si }
    public MostrarTuotial usarTutorial;

    [Header("Paneles del Tutorial")]
    public GameObject panelTutorial1;
    public GameObject panelTutorial2;
    public GameObject panelTutorial3;

    public static bool tutorialActivo;

    private void Start()
    {
        // Si el TutorialSaver decidió que SÍ se muestra, abrimos el panel 1
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
        // Si el tutorial está visible y presionan 'X', se cierra completo
        if (tutorialActivo && Input.GetKeyDown(KeyCode.X))
        {
            CerrarTutorial();
        }
    }

    // Método para abrir el tutorial (sirve también para el botón "Instrucciones" del Menú de Pausa)
    public void AbrirTutorial()
    {
        MostrarPanel(1);
        tutorialActivo = true;
        Time.timeScale = 0f; // Pausa el juego
    }

    // Maneja la visibilidad de las 3 páginas
    public void MostrarPanel(int numeroPanel)
    {
        if (panelTutorial1 != null) panelTutorial1.SetActive(numeroPanel == 1);
        if (panelTutorial2 != null) panelTutorial2.SetActive(numeroPanel == 2);
        if (panelTutorial3 != null) panelTutorial3.SetActive(numeroPanel == 3);
    }

    // Funciones para conectar directamente en las flechas de los botones
    public void IrAPanel1() => MostrarPanel(1);
    public void IrAPanel2() => MostrarPanel(2);
    public void IrAPanel3() => MostrarPanel(3);

    public void CerrarTutorial()
    {
        OcultarTodosLosPaneles();
        tutorialActivo = false;
        Time.timeScale = 1f; // Reanuda el juego
    }

    private void OcultarTodosLosPaneles()
    {
        if (panelTutorial1 != null) panelTutorial1.SetActive(false);
        if (panelTutorial2 != null) panelTutorial2.SetActive(false);
        if (panelTutorial3 != null) panelTutorial3.SetActive(false);
    }
}