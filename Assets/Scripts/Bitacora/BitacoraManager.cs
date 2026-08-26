using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BitacoraManager : MonoBehaviour
{
    public static BitacoraManager Instance { get; private set; }

    [Header("Panel de ficha")]
    [SerializeField] private GameObject panelFicha;

    [Header("Información del personaje")]
    [SerializeField] private TMP_Text nombreText;
    [SerializeField] private Image retratoImage;
    [SerializeField] private TMP_Text informacionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        panelFicha = gameObject;

        panelFicha.SetActive(false);
    }

    public void AbrirFicha(SuspectData suspect)
    {
        if (suspect == null)
        {
            Debug.LogWarning("[BitacoraManager] Se intentó abrir una ficha vacía.");
            return;
        }

        // Nombre
        if (nombreText != null)
        {
            nombreText.text = suspect.suspectName;
        }

        // Retrato
        if (retratoImage != null)
        {
            retratoImage.sprite = suspect.portrait;
        }

        // Información
        ConstruirInformacion(suspect);

        // Abrir panel
        if (panelFicha != null)
        {
            panelFicha.SetActive(true);
        }

        Debug.Log("[BitacoraManager] Ficha abierta: " + suspect.suspectName);
    }

    private void ConstruirInformacion(SuspectData suspect)
    {
        if (informacionText == null)
            return;

        StringBuilder textoFinal = new StringBuilder();

        if (suspect.notebookEntries == null)
        {
            informacionText.text = "";
            return;
        }

        foreach (NotebookEntry entrada in suspect.notebookEntries)
        {
            if (entrada == null)
                continue;

            if (entrada.flag == null)
                continue;

            if (GameStateManager.Instance != null &&
                GameStateManager.Instance.TieneBandera(entrada.flag))
            {
                if (!string.IsNullOrWhiteSpace(entrada.texto))
                {
                    if (textoFinal.Length > 0)
                    {
                        textoFinal.Append("\n\n");
                    }

                    textoFinal.Append(entrada.texto);
                }
            }
        }

        informacionText.text = textoFinal.ToString();
    }

    public void CerrarFicha()
    {
        if (panelFicha != null)
        {
            panelFicha.SetActive(false);
        }
    }
}