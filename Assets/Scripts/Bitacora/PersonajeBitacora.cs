using UnityEngine;
using UnityEngine.InputSystem;

public class PersonajeBitacora : MonoBehaviour
{
    [Header("Datos del personaje")]
    [SerializeField] private SuspectData suspectData;

    [Header("Configuración")]
    [SerializeField] private GameFlag flagDesbloqueo;

    private DetectorHover detectorHover;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (detectorHover == null || !detectorHover.MouseEstaEncima)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            AbrirFicha();
        }
    }

    private void AbrirFicha()
    {
        if (BitacoraManager.Instance == null)
        {
            Debug.LogError("[PersonajeBitacora] No existe BitacoraManager.");
            return;
        }

        BitacoraManager.Instance.AbrirFicha(suspectData);
    }
}