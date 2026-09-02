using UnityEngine;
using UnityEngine.EventSystems;

public class InspectionArea : MonoBehaviour, IPointerEnterHandler
{
    [Header("Zona de inspección")]
    public InspectionZone zone;

    [Header("Manager")]
    public InspectionManager inspectionManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("[InspectionArea] Entró a zona: " + zone);

        if (inspectionManager == null)
        {
            Debug.LogError("[InspectionArea] No hay InspectionManager asignado en " + gameObject.name);
            return;
        }

        inspectionManager.InspectZone(zone);
    }
}