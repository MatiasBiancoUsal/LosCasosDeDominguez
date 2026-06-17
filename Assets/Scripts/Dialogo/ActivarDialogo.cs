using UnityEngine;
using UnityEngine.InputSystem;

public class ActivarDialogo : MonoBehaviour
{
    public DialogoSistema conversacion;

    private DetectorHover hover;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();

        if (hover == null)
        {
            Debug.LogError("No se encontró DetectorHover en " + gameObject.name);
        }
    }

    private void Update()
    {
        // Solo abre el diálogo si el mouse está sobre ESTE personaje
        if (hover != null &&
            hover.MouseEstaEncima &&
            Keyboard.current.iKey.wasPressedThisFrame)
        {
            DialogoManager.Instance.IniciarDialogo(conversacion);

            Debug.Log("Diálogo activado: " + conversacion.name);
        }
    }
}