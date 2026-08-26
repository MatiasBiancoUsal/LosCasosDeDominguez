using UnityEngine;
using UnityEngine.InputSystem;

public class BitacoraUI : MonoBehaviour
{
    [Header("Panel de Bitácora")]
    [SerializeField] private GameObject panelBitacora;

    private void Start()
    {
        if (panelBitacora != null)
        {
            panelBitacora.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            AlternarBitacora();
        }
    }

    private void AlternarBitacora()
    {
        if (panelBitacora == null)
            return;

        bool estaAbierta = panelBitacora.activeSelf;

        panelBitacora.SetActive(!estaAbierta);

        Debug.Log(estaAbierta
            ? "[Bitácora] Cerrada."
            : "[Bitácora] Abierta.");
    }
}