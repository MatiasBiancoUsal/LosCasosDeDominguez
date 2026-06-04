using UnityEngine;
using UnityEngine.InputSystem;

public class ActivarPanelMouse : MonoBehaviour
{
    [Header("Configuración del Panel")]
    [SerializeField] private GameObject panelPrefab; 
    [SerializeField] private DialogoSistema datosScriptableObject; 

    private bool mouseEncima = false;
    private GameObject panelInstanciado;

    void Update()
    {
       
        if (mouseEncima && Keyboard.current.rKey.wasPressedThisFrame)
        {
            AbrirPanelPrefab();
        }
    }

   
    private void OnMouseEnter()
    {
        mouseEncima = true;
    }

    
    private void OnMouseExit()
    {
        mouseEncima = false;
    }

    private void AbrirPanelPrefab()
    {
        if (panelPrefab != null && panelInstanciado == null)
        {
           
            panelInstanciado = Instantiate(panelPrefab);
            Debug.Log("Panel prefab abierto con la tecla r");

           
        }
    }
}
