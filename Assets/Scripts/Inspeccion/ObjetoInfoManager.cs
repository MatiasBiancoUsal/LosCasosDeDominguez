using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjetoInfoManager : MonoBehaviour
{
    public static ObjetoInfoManager Instance;

    [Header("Componentes de la UI")]
    public GameObject panelInfo;
    public TextMeshProUGUI textoNombre;
    public Image imgObjeto;

    private void Awake()
    {
        Instance = this;

        if (panelInfo != null)
        {
            panelInfo.SetActive(false);
        }
    }

    public void MostrarInfo(PistasScriptable data)
    {
        if (data == null)
        {
            Debug.LogError("¡Le estás pasando un ScriptableObject vacío al Manager!");
            return;
        }

        textoNombre.text = data.nombreObjeto;
        imgObjeto.sprite = data.imagenObjeto;

        panelInfo.SetActive(true);
        Debug.Log("¡El Manager activó el panel para: " + data.nombreObjeto + "!");
    }

    public void CerrarPanel()
    {
        if (panelInfo != null)
        {
            panelInfo.SetActive(false);
        }
    }
}