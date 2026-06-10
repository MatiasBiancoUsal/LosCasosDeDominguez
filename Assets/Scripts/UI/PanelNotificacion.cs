using UnityEngine;
using UnityEngine.Events;

public class PanelNotificacion : MonoBehaviour
{
    public Vector2 _timer = new Vector2(15, 30);
    public GameObject _pantallaNotifi;
    public PanelMisiones _panelMisiones;

    public UnityEvent _alAcabarElTimer;

    private void Start()
    {
        float tim = Random.Range(_timer.x, _timer.y);

        Invoke(nameof(EjecutarEvento), tim);
    }

    void EjecutarEvento()
    {
        _alAcabarElTimer.Invoke();
    }

    public void AbrirNotifi()
    {
        if (!EstadoNotifi)
        {
            EstadoNotifi = true;
        }
        else
        {
            EstadoNotifi = false;
        }
    }

    public void AbrirMisiones()
    {
        _panelMisiones.EstadoPanel = true;
    }

    public bool EstadoNotifi { get { return _pantallaNotifi.activeInHierarchy; } set { _pantallaNotifi.SetActive(value); } }
}
