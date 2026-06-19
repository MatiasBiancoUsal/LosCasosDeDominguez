using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;

    [Header("Inventario")]
    [SerializeField] private GameObject inventarioPistas;

    [Header("Pista desbloqueada")]
    [SerializeField] private GameObject pistaNueva;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CerrarTutorial();
        }
    }

    private void CerrarTutorial()
    {
        if (tutorialUI != null)
        {
            tutorialUI.SetActive(false);
        }

        // Desbloquea la pista
        if (pistaNueva != null)
        {
            pistaNueva.SetActive(true);
        }

        // Abre el inventario
        if (inventarioPistas != null)
        {
            inventarioPistas.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
