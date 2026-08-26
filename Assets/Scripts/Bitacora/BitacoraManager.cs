using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BitacoraManager : MonoBehaviour
{
    public static BitacoraManager Instance { get; private set; }

    [Header("Ficha del personaje")]
    [SerializeField] private Image retrato;
    [SerializeField] private TMP_Text nombre;
    [SerializeField] private TMP_Text informacion;

    [Header("Panel")]
    [SerializeField] private GameObject panelFicha;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AbrirFicha(SuspectData suspect)
    {
        if (suspect == null)
        {
            Debug.LogWarning("[BitacoraManager] El sospechoso es nulo.");
            return;
        }

        Debug.Log("Mostrando ficha de: " + suspect.suspectName);

        // Nombre
        if (nombre != null)
        {
            nombre.text = suspect.suspectName;
        }

        // Retrato
        if (retrato != null)
        {
            retrato.sprite = suspect.portrait;
        }

        // Información
        if (informacion != null)
        {
            informacion.text = "";

            foreach (NotebookEntry entry in suspect.notebookEntries)
            {
                if (entry.flag == null)
                    continue;

                if (GameStateManager.Instance != null &&
                    GameStateManager.Instance.TieneBandera(entry.flag))
                {
                    informacion.text += "• " + entry.texto + "\n\n";
                }
            }
        }

        // Abrir ficha
        if (panelFicha != null)
        {
            panelFicha.SetActive(true);
        }
    }
}