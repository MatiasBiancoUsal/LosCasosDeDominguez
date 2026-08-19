using UnityEngine;

public class AccionRecogerObjeto : MonoBehaviour, IAccionInteractuable
{
    public void EjecutarAccion()
    {
        Destroy(gameObject);
    }
}