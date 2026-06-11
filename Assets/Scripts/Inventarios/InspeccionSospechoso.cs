using UnityEngine;
using UnityEngine.InputSystem;

public class InspeccionSospechoso : MonoBehaviour
{
    [Header("Panel de la UI de Sospechoso")]
    [SerializeField] private GameObject panelSospechoso;

    private bool justoSeAbrio;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (panelSospechoso == null || !panelSospechoso.activeSelf) return;

        if (justoSeAbrio)
        {
            justoSeAbrio = false;
            return;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Cerrar();
        }
    }

    public void Abrir()
    {
        if (panelSospechoso == null) return;

        panelSospechoso.SetActive(true);
        Time.timeScale = 0f;
        justoSeAbrio = true;
    }

    public void Cerrar()
    {
        if (panelSospechoso == null) return;

        panelSospechoso.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool EstaAbierto()
    {
        return panelSospechoso != null && panelSospechoso.activeSelf;
    }
}