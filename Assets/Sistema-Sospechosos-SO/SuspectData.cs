using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Panel_Inspeccion/Suspect")]
public class SuspectData : ScriptableObject
{
    [Header("Información del personaje")]
    public string suspectName;

    public Sprite portrait;

    [Header("Desbloqueo en Bitácora")]
    [Tooltip("El personaje aparecerá en la Bitácora cuando esta bandera esté desbloqueada.")]
    public GameFlag flagDesbloqueo;

    [Header("Pistas de inspección")]
    public List<ClueData> clues;

    [Header("Información de Bitácora")]
    public List<NotebookEntry> notebookEntries;

    public ClueData GetClueByZone(InspectionZone zone)
    {
        foreach (ClueData clue in clues)
        {
            if (clue.zone == zone)
                return clue;
        }

        return null;
    }
}