using UnityEngine;

public class InspeccionFoto : MonoBehaviour
{
    public GameObject _fotoZoom;

    public void HacerZoom()
    {
        _fotoZoom.SetActive(true);
    }

    public void CerrarZoom()
    {
        _fotoZoom.SetActive(false);
    }   
}
