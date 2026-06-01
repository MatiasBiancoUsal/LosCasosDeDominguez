using UnityEngine;
using UnityEngine.InputSystem;


public class ActivarDialogo : MonoBehaviour
{
    public DialogoSistema conversacion;

 

void Update()
{
    // 2. Usa esta nueva sintaxis (ejemplo con la tecla 'E')
    if (Keyboard.current.iKey.wasPressedThisFrame)
    {
            DialogoManager.Instance.IniciarDialogo(conversacion);
            Debug.Log("Dialogo activado");


        }
    }


   // public void OnMouseDown()
    //{
       // DialogoManager.Instance.IniciarDialogo(conversacion);
       // Debug.Log("Dialogo activado");
    //}
}