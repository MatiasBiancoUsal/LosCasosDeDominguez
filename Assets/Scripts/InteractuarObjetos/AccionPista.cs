using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class AccionPista : MonoBehaviour, IAccionInteractuable
{
    [Header("Configuraci�n de Pista")]
    [SerializeField] private PistasScriptable datosDelObjeto;

    private bool panelAbierto = false;

    private void Update()
    {
        // Cerrar panel con la tecla X
        if (panelAbierto && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CerrarPanel();
        }
    }

    public void EjecutarAccion()
    {
        if (ObjetoInfoManager.Instance == null || datosDelObjeto == null)
        {
            Debug.LogWarning($"[AccionPista] Falta ObjetoInfoManager o PistasScriptable en: {gameObject.name}");
            return;
        }

        ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);
        panelAbierto = true;

        ObjetoInfoManager.Instance.MostrarInfo(datosDelObjeto);
        panelAbierto = true;

        // ANALYTICS - Maia
        string nombreObjeto = datosDelObjeto.nombreObjeto;
        string nombreHabitacion = SceneManager.GetActiveScene().name;

        print($"[Analytics] InspeccionarObjeto -> NombreObjeto: {nombreObjeto}, NombreHabitacion: {nombreHabitacion}");

        // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (Maia)
        EnviarDato.Singleton.EventoInspeccionarObjeto(nombreObjeto, nombreHabitacion);
    }

    public void CerrarPanel()
    {
        if (ObjetoInfoManager.Instance != null)
        {
            ObjetoInfoManager.Instance.CerrarPanel();
        }
        panelAbierto = false;
    }

    public void ForzarCierreDesdeUI()
    {
        CerrarPanel();
    }
}

