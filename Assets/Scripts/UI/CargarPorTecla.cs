using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarPorTecla : MonoBehaviour
{
    public KeyCode _tecla = KeyCode.N;
    public string _escena;

    private void Update()
    {
        if (Input.GetKeyDown(_tecla))
        {
            SceneManager.LoadScene(_escena);
        }
    }
}
