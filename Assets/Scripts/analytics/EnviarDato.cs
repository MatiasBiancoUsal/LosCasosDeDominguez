using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class EnviarDato : MonoBehaviour
{
    public static EnviarDato Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AnalyticsService.Instance.StartDataCollection();
        }
        catch
        {
            ///
        }
    }

    // 1-Pilar. Activar el evento cuando se activa el dialogo
    public void EventoDialogo(string nombrePersonaje, string nombreHabitacion, string nombreDialogo, bool esRepetido)
    {
        CustomEvent empezarDialogo = new CustomEvent("DialogoPersonaje")
        {
            { "NombreDelSospechoso", nombrePersonaje },
            { "NombreHabitacion", nombreHabitacion },
            { "NombreDialogo", nombreDialogo },
            { "DialogoRepetido", esRepetido }
        };

        AnalyticsService.Instance.RecordEvent(empezarDialogo);
    }

    // 1-Dolores. Activar el evento cuando el jugador elige al culpable final
    public void EventoElegirCulpable(string finalElegido)
    {
        CustomEvent eventoCulpable = new CustomEvent("ElegirCulpable")
        {
            { "FinalElegido", finalElegido }
        };

        AnalyticsService.Instance.RecordEvent(eventoCulpable);
    }

    // 2-Dolores. Activar el evento cuando el usuario realiza todos los interrogatorios finales
    public void EventoInterrogacionFinal(string nombreSospechoso, string nombreDialogo)
    {
        CustomEvent eventoInterrogacion = new CustomEvent("InterrogacionFinal")
        {
            { "NombreDelSospechoso", nombreSospechoso },
            { "NombreDialogo", nombreDialogo }
        };

        AnalyticsService.Instance.RecordEvent(eventoInterrogacion);
    }

    // 1-Isa. Activar el evento cuando el jugador acepta o rechaza la llamada
    public void EventoAtenderLlamada(bool aceptoLlamada)
    {
        CustomEvent eventoLlamada = new CustomEvent("AtenderLlamada")
        {
            { "AceptarLlamada", aceptoLlamada }
        };

        AnalyticsService.Instance.RecordEvent(eventoLlamada);
    }

    // 2-Isa. Activar el evento cuando el jugador entra a una habitacion
    public void EventoEntrarHabitacion(string nombreHabitacion, float tiempoDeJuego)
    {
        CustomEvent eventoHabitacion = new CustomEvent("EntrarHabitacion")
        {
            { "NombreHabitacion", nombreHabitacion },
            { "TiempoDeJuego", tiempoDeJuego }
        };

        AnalyticsService.Instance.RecordEvent(eventoHabitacion);
    }

    // 3-Isa. Activar el evento cuando el jugador intenta abrir una puerta cerrada
    public void EventoPuertaBloqueada(string nombreHabitacion, string habitacionDestino, string pistaFaltante)
    {
        CustomEvent eventoPuerta = new CustomEvent("PuertaBloqueada")
        {
            { "NombreHabitacion", nombreHabitacion },
            { "HabitacionDestino", habitacionDestino },
            { "PistaFaltante", pistaFaltante }
        };

        AnalyticsService.Instance.RecordEvent(eventoPuerta);
    }

    //  1-Mora. Activar el evento cuando el jugador agarra una llave
    /// </summary>
    /// <param name="llave"></param>
    /// <param name="habitacion"></param>
    public void EventoNombreLlave(string llave, string habitacion)
    {
        CustomEvent llaves = new("agarrasteLaLlave")
        {
            {"Llave: ", "agarrasteLaLlave" },
            {"Habitacion: ", habitacion}
        };
    }

    // 1-Cande. Activar el evento cuando se completa una mision
    public void EventoMisionCompletada(string nombreMision, float tiempoMision)
    {
        CustomEvent eventoMision = new CustomEvent("MisionCompletada")
        {
            { "NombreMision", nombreMision },
            { "TiempoMision", tiempoMision }
        };

        AnalyticsService.Instance.RecordEvent(eventoMision);
    }

    // 1-Lu. Activar el evento cuando aparece una mision nueva
    public void EventoNuevaMision(string nombreMision)
    {
        CustomEvent eventoMision = new CustomEvent("MisionIniciada")
        {
            { "NombreMision", nombreMision }
        };

        AnalyticsService.Instance.RecordEvent(eventoMision);
    }

    // 2-Lu. Activar el evento cuando el jugador abre el panel de misiones
    public void EventoAbrirPanelMisiones(string nombreHabitacion)
    {
        CustomEvent eventoPanel = new CustomEvent("AbrirMision")
        {
            { "NombreHabitacion", nombreHabitacion }
        };

        AnalyticsService.Instance.RecordEvent(eventoPanel);
    }

    // 1-Valen. Activar el evento cuando el jugador interactua con un objeto antes de tiempo
    public void EventoInteraccionBloqueada(string nombreObjeto, string pistaFaltante)
    {
        CustomEvent eventoBloqueo = new CustomEvent("InteraccionBloqueada")
        {
            { "NombreObjeto", nombreObjeto },
            { "PistaFaltante", pistaFaltante }
        };

        AnalyticsService.Instance.RecordEvent(eventoBloqueo);
    }

    //1- Maia. Activar el evento cuando el jugador inspecciona un sospechoso.
    public void EventoInspeccionarSospechoso(string nombreSospechoso)
    {
        CustomEvent eventoSospechoso = new CustomEvent("InspeccionarSospechoso")
        {
            { "NombreDelSospechoso", nombreSospechoso }
        };

        AnalyticsService.Instance.RecordEvent(eventoSospechoso);
    }

    //2- Maia. Activar el evento cuando el jugador inspecciona un objeto.
    public void EventoInspeccionarObjeto(string nombreObjeto, string nombreHabitacion)
    {
        CustomEvent eventoObjeto = new CustomEvent("InspeccionarObjeto")
        {
            { "NombreObjeto", nombreObjeto },
            { "NombreHabitacion", nombreHabitacion }
        };

        AnalyticsService.Instance.RecordEvent(eventoObjeto);
    }
}