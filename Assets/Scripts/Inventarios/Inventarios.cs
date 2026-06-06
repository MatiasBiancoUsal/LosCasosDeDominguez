using UnityEngine;
using UnityEngine.InputSystem;

public class Inventarios : MonoBehaviour
{
    [SerializeField] private GameObject PanelInvPistas;
    [SerializeField] private GameObject PanelInvSospechosos;

    private bool mouseEncima = false;

    private void Update()
    {
        if (mouseEncima && Keyboard.current != null)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                PanelInvPistas.SetActive(!PanelInvPistas.activeSelf);
                ActualizarEstadoTiempo(); //si se pausa o despausa
            }

            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                PanelInvSospechosos.SetActive(!PanelInvSospechosos.activeSelf);
                ActualizarEstadoTiempo(); 
            }
        }
    }

    private void ActualizarEstadoTiempo()
    {
        if (PanelInvPistas.activeSelf || PanelInvSospechosos.activeSelf)
        {
            Time.timeScale = 0f; // Pausa el juego
            Debug.Log("Juego Pausado");
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el juego
            Debug.Log("Juego Reanudado");
        }
    }

    private void OnMouseEnter()
    {
        mouseEncima = true;
    }

    private void OnMouseExit()
    {
        mouseEncima = false;
    }
}
