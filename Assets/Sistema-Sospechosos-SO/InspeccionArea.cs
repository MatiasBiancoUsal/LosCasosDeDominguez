using UnityEngine;
using UnityEngine.EventSystems;

public class InspectionArea : MonoBehaviour, IPointerEnterHandler
{
    public InspectionZone zone;

    public InspectionManager inspectionManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entró a zona: " + zone);

        inspectionManager.InspectZone(zone);
    }




}