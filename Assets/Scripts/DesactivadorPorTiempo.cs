using UnityEngine;

public class DesactivadorPorTiempo : MonoBehaviour
{
    public GameObject _target;
    public float _tiempo = 5;

    private void OnEnable()
    {
        Invoke(nameof(Desactivar), _tiempo);
    }

    void Desactivar()
    {
        _target.SetActive(false);
    }
}
