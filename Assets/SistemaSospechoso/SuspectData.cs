using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Panel_Inspeccion/Suspect")]
public class SuspectData : ScriptableObject
{
    public string suspectName;

    public Sprite portrait;

    public List<ClueData> clues;

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