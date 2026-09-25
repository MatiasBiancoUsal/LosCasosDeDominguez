using UnityEngine;

public class ObjetoZoom : MonoBehaviour
{
    bool inZoom;
    public RectTransform _objeto2D;
    public Vector2 outZoomScale = new(100, 100);
    public Vector2 inZoomScale = new(200f, 200f);
    public float _zoomSpeed = 5f;

    private void Update()
    {
        if (inZoom)
        {
            _objeto2D.sizeDelta = Vector2.Lerp(_objeto2D.sizeDelta, inZoomScale, Time.deltaTime * _zoomSpeed);
        }
        else
        {
            _objeto2D.sizeDelta = Vector2.Lerp(_objeto2D.sizeDelta, outZoomScale, Time.deltaTime * _zoomSpeed);
        }
    }

    public void Ver()
    {
        if (!inZoom)
        {
            inZoom = true;
        }
        else
        {
            inZoom = false;
        }
    }
}
