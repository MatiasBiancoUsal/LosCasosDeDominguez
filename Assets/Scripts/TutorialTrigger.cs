using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;

    private void OnMouseDown()
    {
        tutorialUI.SetActive(false);

        gameObject.SetActive(false);
    }
}
