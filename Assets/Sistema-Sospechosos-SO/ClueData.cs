using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "Panel_Inspeccion/ClueData")]
public class ClueData : ScriptableObject
{
    [Header("Información de la pista")]
    public string clueName;

    [TextArea]
    public string description;

    [Header("Zona de inspección")]
    public InspectionZone zone;

    [Header("Persistencia")]
    [Tooltip("Bandera que se guarda cuando el jugador descubre esta pista.")]
    public GameFlag flagDesbloqueo;
}