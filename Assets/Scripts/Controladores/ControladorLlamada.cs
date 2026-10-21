using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;



public class ControladorLlamada : MonoBehaviour
{
    public void AceptarLlamada()
    {
        print($"Evento AtenderLlamada. Parametro aceptarLlamada: {true}");
        CustomEvent evento = new CustomEvent("AtenderLlamada")
        {
            { "aceptarLlamada", true }
        };
        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush();

        SceneManager.LoadScene(3);
    }

    public void RechazarLlamada()
    {
        print($"Evento AtenderLlamada. Parametro aceptarLlamada: {false}");
        CustomEvent evento = new CustomEvent("AtenderLlamada")
        {
            { "aceptarLlamada", false }
        };
        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush();

        SceneManager.LoadScene("Menu");
    }
}

