using UnityEngine;

public class ControladorTutorial : MonoBehaviour
{
    public enum MostrarTuotial { no, [InspectorName("Sí")]si }
    public MostrarTuotial usarTutorial;

    [Header("Panel del tutorial")]
    public GameObject panelTutorial;
    public GameObject SegundoTutorial;

    public static bool tutorialActivo;

    private void Start()
    {
        if (usarTutorial == MostrarTuotial.si)
        {
            panelTutorial.SetActive(true);
        }

        tutorialActivo = true;

        Time.timeScale = 0f;
    }

    public void PasarASegundoTutorial()
    {
        panelTutorial.SetActive(false);
        SegundoTutorial.SetActive(true);

        Time.timeScale = 1f;
    }

    public void CerrarTutorial()
    {
        panelTutorial.SetActive(false);
        SegundoTutorial.SetActive(false);

        tutorialActivo = false;

        Time.timeScale = 1f;
    }
}