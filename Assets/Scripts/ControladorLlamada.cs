using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorLlamada : MonoBehaviour
{
    public void AceptarLlamada()
    {
        SceneManager.LoadScene(3);
    }

    public void RechazarLlamada()
    {
        SceneManager.LoadScene("Menu");
    }
}


