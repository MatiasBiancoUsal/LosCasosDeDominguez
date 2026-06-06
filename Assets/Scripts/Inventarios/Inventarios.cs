using UnityEngine;

public class Inventarios : MonoBehaviour
{
    [SerializeField] private GameObject PanelInvPistas;
    [SerializeField] private GameObject PanelInvSospechosos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PanelInvPistas.SetActive(!PanelInvPistas.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PanelInvSospechosos.SetActive(!PanelInvSospechosos.activeSelf);
        }
    }
}
