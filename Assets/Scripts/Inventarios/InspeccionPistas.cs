using UnityEngine;
using UnityEngine.InputSystem;

public class InspeccionPistas : MonoBehaviour
{
    [Header("Panel de la UI de Pistas")]
    [SerializeField] private GameObject panelPistas;


    private void Update()
    {
        if (Keyboard.current == null) return;

        if (panelPistas == null || !panelPistas.activeSelf) return;


        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Cerrar();
        }
    }

    public void Abrir()
    {
        if (panelPistas == null) return;

        panelPistas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Cerrar()
    {
        if (panelPistas == null) return;

        panelPistas.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool EstaAbierto()
    {
        return panelPistas != null && panelPistas.activeSelf;
    }
}