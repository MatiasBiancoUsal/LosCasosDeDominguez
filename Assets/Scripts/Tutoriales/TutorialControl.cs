using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialControl : MonoBehaviour
{
    [Header("Panel del tutorial")]
    public GameObject panelTutorial;
    public GameObject SegundoTutorial;

    public static bool tutorialActivo;

    public void PasarASegundoTutorial2()
    {
        panelTutorial.SetActive(false);
        SegundoTutorial.SetActive(true);

        Time.timeScale = 1f;
    }

    public void CerrarTutorial2()
    {
        panelTutorial.SetActive(false);
        SegundoTutorial.SetActive(false);

        tutorialActivo = false;

        Time.timeScale = 1f;
    }
}