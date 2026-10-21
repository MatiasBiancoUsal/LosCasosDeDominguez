using Unity.Services.Analytics;
using UnityEngine;

public class EnviarDato : MonoBehaviour
{
    public static EnviarDato Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    // 1-Pilar. Activar el evento cuando se activa el dialogo
    public void EventoDialogo(string nombrePersonaje, string nombreHabitacion, string nombreDialogo, bool esRepetido)
    {
        CustomEvent empezarDialogo = new CustomEvent("Dialogo")
        {
            { "personaje_seleccionado", nombrePersonaje },
            { "nombre_habitacion", nombreHabitacion },
            { "nombre_dialogo", nombreDialogo },
            { "es_repetido", esRepetido }
        };

        AnalyticsService.Instance.RecordEvent(empezarDialogo);
    }

    // 1-Dolores. Activar el evento cuando el jugador elige al culpable final
    public void EventoElegirCulpable(string finalElegido)
    {
        CustomEvent eventoCulpable = new CustomEvent("ElegirCulpable")
        {
            { "Final elegido", finalElegido }
        };

        AnalyticsService.Instance.RecordEvent(eventoCulpable);

    }

    // 2-Dolores. Activar el evento cuando el usuario realiza todos los interrogatorios finales
    public void EventoInterrogacionFinal(float nombreSospechoso, float nombreDialogo)
    {
        CustomEvent eventoInterrogacion = new CustomEvent("InterrogacionFinal")
        {
            { "Nombre del sospechoso", nombreSospechoso },
            { "Nombre del dialogo", nombreDialogo }
        };

        AnalyticsService.Instance.RecordEvent(eventoInterrogacion);

    }
}