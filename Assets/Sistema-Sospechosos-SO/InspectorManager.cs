using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InspectionManager : MonoBehaviour
{
    [Header("Sospechoso actual")]
    public SuspectData currentSuspect;

    [Header("UI")]
    public TMP_Text cluesText;
    public TMP_Text suspectNameText;
    public Image suspectPortraitImage;

    [Header("Panel")]
    [SerializeField] private GameObject inspectionPanel;

    private HashSet<ClueData> discoveredClues = new HashSet<ClueData>();

    // Memoria de pistas descubiertas por sospechoso
    private Dictionary<SuspectData, HashSet<ClueData>> suspectDiscoveries =
        new Dictionary<SuspectData, HashSet<ClueData>>();

    // Evita que la misma Q que abre el panel lo cierre instantáneamente
    private bool ignoreNextQ = false;

    private void Start()
    {
        if (currentSuspect != null)
        {
            SetSuspect(currentSuspect);
        }

        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (ignoreNextQ)
        {
            ignoreNextQ = false;
            return;
        }

        if (inspectionPanel == null)
            return;

        if (!inspectionPanel.activeSelf)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            inspectionPanel.SetActive(false);

            Debug.Log("Panel de inspección cerrado.");
        }
    }

    public void InspectZone(InspectionZone zone)
    {
        if (currentSuspect == null)
        {
            Debug.LogWarning("No hay sospechoso asignado.");
            return;
        }

        ClueData clue = currentSuspect.GetClueByZone(zone);

        if (clue == null)
        {
            Debug.Log("No se encontró ninguna pista.");
            return;
        }

        if (discoveredClues.Contains(clue))
        {
            return;
        }

        discoveredClues.Add(clue);

        AddClueToUI(clue);

        Debug.Log("Pista encontrada: " + clue.clueName);
    }

    private void AddClueToUI(ClueData clue)
    {
        if (cluesText == null)
            return;

        cluesText.text +=
            "• " + clue.clueName + "\n" +
            clue.description + "\n\n";
    }

    private void RebuildClueUI()
    {
        if (cluesText == null)
            return;

        cluesText.text = "";

        foreach (ClueData clue in discoveredClues)
        {
            AddClueToUI(clue);
        }
    }

    public void SetSuspect(SuspectData suspect)
    {
        if (suspect == null)
        {
            Debug.LogWarning("SetSuspect recibió un sospechoso nulo.");
            return;
        }

        Debug.Log("SetSuspect llamado con " + suspect.suspectName);

        // Guardar progreso del sospechoso anterior
       // if (currentSuspect != null)
        //{
         //   suspectDiscoveries[currentSuspect] =
          //      new HashSet<ClueData>(discoveredClues);
       // }

        currentSuspect = suspect;

        // Recuperar progreso previo si existe
        if (suspectDiscoveries.ContainsKey(suspect))
        {
            discoveredClues =
                new HashSet<ClueData>(suspectDiscoveries[suspect]);
        }
        else
        {
            discoveredClues = new HashSet<ClueData>();
        }

        // Actualizar UI
        if (suspectNameText != null)
        {
            suspectNameText.text = suspect.suspectName;
        }

        if (suspectPortraitImage != null)
        {
            suspectPortraitImage.sprite = suspect.portrait;
        }

        RebuildClueUI();

        // Si hay panel asignado, lo abrimos
        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(true);
        }

        // Ignorar la misma Q que abrió el panel
        ignoreNextQ = true;

        Debug.Log("Inspeccionando a: " + suspect.suspectName);
    }

    private void OnDisable()
    {
        Debug.Log("InspectionManager desactivado.");
    }
}