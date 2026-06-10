using UnityEngine;

public class ClickParteCuerpo : MonoBehaviour
{
    [SerializeField] private DesbloquearPistaTexto textoADesbloquear;

    private void OnMouseDown()
    {
         Debug.Log("CLICK DETECTADO");
         
        if (textoADesbloquear != null)
        {
            textoADesbloquear.Desbloquear();
        }
    }
}