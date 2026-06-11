using UnityEngine;

public class ClickParteCuerpo : MonoBehaviour
{
    [SerializeField] private DesbloquearPistaTexto textoADesbloquear;

    private void OnMouseEnter()
    {
        Debug.Log("MOUSE ENCIMA");

        if (textoADesbloquear != null)
        {
            textoADesbloquear.Desbloquear();
        }
    }
}