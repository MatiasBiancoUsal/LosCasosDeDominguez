using UnityEngine;
using UnityEngine.InputSystem;

public class AccionPista : MonoBehaviour, IAccionInteractuable
{
    [Header("Configuración de Pista")]
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

