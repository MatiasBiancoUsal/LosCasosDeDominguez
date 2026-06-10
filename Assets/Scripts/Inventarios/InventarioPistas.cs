using UnityEngine;
using UnityEngine.InputSystem;

public class InventarioPistas : MonoBehaviour
{
    [Header("Panel de la UI de Pistas")]
    [SerializeField] private GameObject panelPistas;

    private bool justoSeAbrio = false;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (panelPistas != null && panelPistas.activeSelf)
        {
            if (justoSeAbrio)
            {
                justoSeAbrio = false;
                return;
            }

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                Cerrar();
            }
        }
    }

    public void Abrir()
    {
        if (panelPistas != null)
        {
            panelPistas.SetActive(true);
            Time.timeScale = 0f; 
            justoSeAbrio = true; 
            Debug.Log("¡InventarioPistas ordenó ENCENDER su panel!");
        }
    }

    public void Cerrar()
    {
        if (panelPistas != null)
        {
            panelPistas.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public bool EstaAbierto()
    {
        return panelPistas != null && panelPistas.activeSelf;
    }
}

