using UnityEngine;
using UnityEngine.Events;

public class ActivadorPorTiempo : MonoBehaviour
{
    public UnityEvent accion;
    public float _timer;

    private void Start()
    {
        Invoke(nameof(ActivarAccion), _timer);
    }

    void ActivarAccion()
    {
        accion.Invoke();
    }
}
