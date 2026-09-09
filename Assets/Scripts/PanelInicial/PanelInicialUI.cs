using UnityEngine;

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