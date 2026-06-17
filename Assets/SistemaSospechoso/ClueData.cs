using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "Panel_Inspeccion/ClueData")]
public class ClueData : ScriptableObject
{
    public string clueName;
    
    [TextArea]
    public string description;

    public InspectionZone zone;



}
