using UnityEngine;

public class ObjetoInspeccionable : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup _canvas;

    [Header("Efecto Fade")]
    public Vector2 _transparencia = new Vector2(0f, 1f);
    public float _fadeSpeed = 2f;
    bool _seleccion;

    private void Update()
    {
        if (_seleccion)
        {
            _canvas.alpha = Mathf.MoveTowards(_canvas.alpha, _transparencia.y, Time.deltaTime * _fadeSpeed);
        }
        else
        {
            _canvas.alpha = Mathf.MoveTowards(_canvas.alpha, _transparencia.x, Time.deltaTime * _fadeSpeed);
        }
    }

    public void Mostrar()
    {
        _seleccion = true;
    }

    public void Ocultar()
    {
        _seleccion = false;
    }
}
