using UnityEngine;

public class AbrirGenerico : MonoBehaviour
{
    public DetectorHover detectorHover;
    public GameObject panel;

    private void Update()
    {
        if (detectorHover.MouseEstaEncima)
        {
            if (!panel.activeSelf && Input.GetKeyDown(KeyCode.Q))
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
}
