using UnityEngine;
using UnityEngine.InputSystem;

public class ActivarDialogoObjeto : MonoBehaviour
{
    [Header("Configuración de Diálogo")]
    public DialogoSistema conversacion;

    private bool mouseEncima = false;

    private void Update()
    {
        if (mouseEncima)
        {
            // Este log te dirá si el juego detecta que presionas la R estando encima
            if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (DialogoManager.Instance != null)
                {
                    DialogoManager.Instance.IniciarDialogo(conversacion);
                    //Debug.Log("¡Éxito! Diálogo enviado al DialogoManager.");
                }
                else
                {
                    //Debug.LogError("¡ERROR! El DialogoManager no se encuentra en la escena o no se ha inicializado.");
                    return;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        //Debug.Log($"[MOUSE ENTRÓ] El puntero está sobre: {gameObject.name}");
        mouseEncima = true;
    }

    private void OnMouseExit()
    {
        //Debug.Log($"[MOUSE SALIÓ] El puntero dejó: {gameObject.name}");
        mouseEncima = false;
    }
}