using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectorHover))]
public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Persistencia")]
    [Tooltip("Bandera que se otorga al interactuar con este objeto.")]
    [SerializeField] private GameFlag banderaAOtorgar;

    private DetectorHover detectorHover;
    private IAccionInteractuable accionEspecifica;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
        // Busca si el mismo GameObject tiene un componente con comportamiento específico (Pista o Sospechoso)
        accionEspecifica = GetComponent<IAccionInteractuable>();
    }

    private void Update()
    {
        if (Keyboard.current == null || detectorHover == null || !detectorHover.MouseEstaEncima)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            EjecutarInteraccion();
        }
    }

    private void EjecutarInteraccion()
    {
        // 1. Guardar la Bandera en el GameStateManager
        if (banderaAOtorgar != null)
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.GuardarBandera(banderaAOtorgar);
            }
            else
            {
                Debug.LogError("[ObjetoInteractuable] No existe GameStateManager en la escena.");
            }
        }

        // 2. Ejecutar la acción de abrir panel (si el objeto tiene AccionPista o AccionSospechoso)
        if (accionEspecifica != null)
        {
            accionEspecifica.EjecutarAccion();
        }
    }
}