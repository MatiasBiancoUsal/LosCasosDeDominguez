using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;

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

        
        gameObject.SetActive(false);
    }
}
