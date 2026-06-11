using UnityEngine;

public class InventarioActualizador : MonoBehaviour
{
    public RectTransform _base;
    public float _duracion = 5f;

    GameObject actual;

    public void Actualizar(GameObject item)
    {
        actual = Instantiate(item, _base.position, Quaternion.identity);
        actual.transform.SetParent(_base.transform);

        Invoke(nameof(DestruirItem), _duracion);
    }

    void DestruirItem()
    {
        Destroy(actual);
    }

    public float Duracion { get { return _duracion; } set { _duracion = value; } }
}
