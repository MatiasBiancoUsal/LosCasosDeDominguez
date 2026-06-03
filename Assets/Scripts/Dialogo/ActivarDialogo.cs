using UnityEngine;
using UnityEngine.InputSystem;


public class ActivarDialogo : MonoBehaviour
{
    public DialogoSistema conversacion;

 

void Update()
{
    // activar el dialogo con la i de interrogar :)
    if (Keyboard.current.iKey.wasPressedThisFrame)
    {
            DialogoManager.Instance.IniciarDialogo(conversacion);
            Debug.Log("Dialogo activado");


        }
    }


}