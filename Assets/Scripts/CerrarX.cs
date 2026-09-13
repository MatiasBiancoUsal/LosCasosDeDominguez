using UnityEngine;

public class CerrarX : MonoBehaviour
{
    public GameObject panelRoot;

    private void Update()
    {
        if (panelRoot.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                panelRoot.SetActive(false);
            }
        }
    }
}
