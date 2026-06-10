using UnityEngine;

public class ControladorTutorial : MonoBehaviour
{
    public GameObject panelTutorial;

    private void Start()
    {

        if (panelTutorial != null)
        {
            panelTutorial.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void CerrarTutorial()
    {
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(false);
        }

        Time.timeScale = 1f;

    }
}