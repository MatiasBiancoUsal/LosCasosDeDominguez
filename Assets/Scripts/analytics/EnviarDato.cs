using Unity.Services.Analytics;
using UnityEngine;

public class EnviarDato

{
    public static EnviarDato Singleton;


    private void awake()
    {
       Singleton = this;
    }

    public void EventoDialogo(string nombrePersonaje)
    {
        CustomEvent EmpezarDialogo = new CustomEvent("Dialogo")
        {
            { "personaje seleccionado" , nombrePersonaje }
        };

        AnalyticsService.Instance.RecordEvent(EmpezarDialogo);

    }
}