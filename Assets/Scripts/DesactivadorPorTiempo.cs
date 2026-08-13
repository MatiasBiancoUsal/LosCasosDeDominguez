using UnityEngine;

public class DesactivadorPorTiempo : MonoBehaviour
{
    public GameObject _target;
    public float _tiempo = 5;

    // Arrastrás la puerta desde el Inspector
    public SelectorNivel puertaADesbloquear;

    private void OnEnable()
    {
        // Desbloquea la puerta cuando se activa este objeto
        //if (puertaADesbloquear != null)
        //{
        //    puertaADesbloquear.desbloqueado = true;
        //    Debug.Log("Puerta desbloqueada.");
        //}

        Invoke(nameof(Desactivar), _tiempo);
    }

    void Desactivar()
    {
        _target.SetActive(false);
    }
}
