using UnityEngine;

public class ClickParteCuerpo : MonoBehaviour
{
    [Header("Configuración de la Pista")]
    [SerializeField] private DesbloquearPistaTexto textoADesbloquear;

    public void RegistrarClicParteCuerpo()
    {
        Debug.Log("¡CLICK DETECTADO EN LA PARTE DEL CUERPO: " + gameObject.name + "!");

        if (textoADesbloquear != null)
        {
            textoADesbloquear.Desbloquear();
        }
        else
        {
            Debug.LogWarning("Falta asignar el script de texto en " + gameObject.name);
        }
    }
}