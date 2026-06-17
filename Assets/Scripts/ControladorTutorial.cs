using UnityEngine;

public class ControladorTutorial : MonoBehaviour
{
    [Header("Panel del tutorial")]
    public GameObject panelTutorial;

    public static bool tutorialActivo;

    private void Start()
    {
        panelTutorial.SetActive(true);

        tutorialActivo = true;

        Time.timeScale = 0f;
    }

    public void CerrarTutorial()
    {
        panelTutorial.SetActive(false);

        tutorialActivo = false;

        Time.timeScale = 1f;
    }
}