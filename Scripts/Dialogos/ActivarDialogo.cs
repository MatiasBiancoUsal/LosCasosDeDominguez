using UnityEngine;

public class ActivarDialogo : MonoBehaviour
{
    
    public DialogoSistema conversacion;

    public void OnMouseDown()
    {
        
        DialogoManager.Instance.IniciarDialogo(conversacion);
        Debug.Log("Dialogo activado");
    }
}