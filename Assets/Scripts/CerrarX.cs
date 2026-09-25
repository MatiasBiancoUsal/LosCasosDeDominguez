using UnityEngine;

public class CerrarX : MonoBehaviour
{
    public GameObject panelRoot;
    public bool selfClose = true;

    private void Update()
    {
        if (panelRoot != null)
        {
            if (panelRoot.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    panelRoot.SetActive(false);
                }
            }
        }
        else
        {
            if (selfClose && gameObject.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
