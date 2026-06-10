using UnityEngine;
using UnityEngine.UI;

public class ItemInventarioUI : MonoBehaviour
{
    private Image imagenObjeto;
    [SerializeField] private Color colorBloqueado = new Color(0.2f, 0.2f, 0.2f, 0.8f);

    private void Start()
    {
        imagenObjeto = GetComponent<Image>();

        if (imagenObjeto != null)
        {
            imagenObjeto.color = colorBloqueado;
        }
    }

    public void Desbloquear()
    {
        if (imagenObjeto == null)
        {
            imagenObjeto = GetComponent<Image>();
        }

        if (imagenObjeto != null)
        {
            imagenObjeto.color = Color.white;
            Debug.Log("Desbloqueado con éxito: " + gameObject.name);
        }
        else
        {
            Debug.LogError("¡No se encontró el componente Image en " + gameObject.name + "!");
        }
    }
}
