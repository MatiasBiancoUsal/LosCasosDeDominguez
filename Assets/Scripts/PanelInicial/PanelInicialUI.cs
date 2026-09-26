using UnityEngine;
using UnityEngine.InputSystem;

public class PanelInicialUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelInicial;

    private void Start()
    {
        if (panelInicial != null)
        {
            panelInicial.SetActive(true);
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (panelInicial != null && panelInicial.activeSelf)
            {
                CerrarPanel();
            }
        }
    }

    public void CerrarPanel()
    {
        if (panelInicial != null)
        {
            panelInicial.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}