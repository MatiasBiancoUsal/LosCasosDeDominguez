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
            return;
        }

        textoNombre.text = data.nombreObjeto;
        imgObjeto.sprite = data.imagenObjeto;

        panelInfo.SetActive(true);
    }

    public void CerrarPanel()
    {
        if (panelInfo != null)
        {
            panelInfo.SetActive(false);
        }
    }
}