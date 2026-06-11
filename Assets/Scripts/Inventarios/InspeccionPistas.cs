using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InspeccionPistas : MonoBehaviour
{
    [Header("Panel de la UI de Pistas")]
    [SerializeField] private GameObject panelPistas;

    // eso se puede acoplar a todo sin modificar nada
    public UnityEvent _alTermianInspeccion;

    // el programa sabe cuando concluye
    bool terminoLaInspeccion;

    // con esto consultamos desde otras scripts sin modificar nada :)
    public bool TerminoLaInspeccion { get { return terminoLaInspeccion; } }


    private void Update()
    {
        if (Keyboard.current == null) return;

        if (panelPistas == null || !panelPistas.activeSelf) return;


        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Cerrar();
        }
    }

    public void Abrir()
    {
        if (panelPistas == null) return;

        panelPistas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Cerrar()
    {
        if (panelPistas == null) return;

        panelPistas.SetActive(false);
        Time.timeScale = 1f;

        //Actualiza la consulta
        terminoLaInspeccion = true;

        //Si queremos agregar mas coas para hacer despues de finalziar la inspeccion, las metemos y aquí las activamos
        _alTermianInspeccion.Invoke();
    }

    public bool EstaAbierto()
    {
        return panelPistas != null && panelPistas.activeSelf;
    }
}