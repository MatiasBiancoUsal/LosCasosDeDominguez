using Unity.Services.Analytics;
using UnityEngine;

public class EnviarDato

{
    public void EventoDialogo(string nombrePersonaje)
    {
        CustomEvent EmpezarDialogo = new CustomEvent("Dialogo");
        {
           // { "personaje seleccionado" , nombrePersonaje }
        };
    }
}