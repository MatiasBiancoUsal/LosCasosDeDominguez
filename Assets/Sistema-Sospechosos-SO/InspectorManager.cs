using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectionManager : MonoBehaviour
{
    [Header("Suspect")]
    public SuspectData currentSuspect;

    [Header("UI")]
    public TMP_Text cluesText;
    public TMP_Text suspectNameText;
    public Image suspectPortraitImage;

    private HashSet<ClueData> discoveredClues = new HashSet<ClueData>();


    //memoria de sospechosos para guardar el progreso de pistas descubiertas
    private Dictionary<SuspectData, HashSet<ClueData>> suspectDiscoveries =
        new Dictionary<SuspectData, HashSet<ClueData>>();


    private void Start()
    {
        if (currentSuspect != null)
        {
            SetSuspect(currentSuspect);
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
        cluesText.text +=
            "• " + clue.clueName + "\n" +
            clue.description + "\n\n";
    }


    ///reconstruye la UI de pistas desde cero, útil para cambiar de sospechoso o al iniciar el juego
    private void RebuildClueUI()
    {
        cluesText.text = "";

        foreach (ClueData clue in discoveredClues)
        {
            AddClueToUI(clue);
        }
    }

    public void SetSuspect(SuspectData suspect)
    {
        Debug.Log("SetSuspect llamado con: " + suspect.suspectName);

        // Guardar progreso del sospechoso actual
        if (currentSuspect != null)
        {
            suspectDiscoveries[currentSuspect] =
                new HashSet<ClueData>(discoveredClues);
        }

        currentSuspect = suspect;

        // Recuperar progreso anterior
        if (suspectDiscoveries.ContainsKey(suspect))
        {
            discoveredClues =
                new HashSet<ClueData>(suspectDiscoveries[suspect]);
        }
        else
        {
            discoveredClues = new HashSet<ClueData>();
        }

        suspectNameText.text = suspect.suspectName;
        suspectPortraitImage.sprite = suspect.portrait;

        RebuildClueUI();
    }
}
