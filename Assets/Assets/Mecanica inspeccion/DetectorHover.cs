using UnityEngine;

public class DetectorHover : MonoBehaviour
{
   
    public GameObject botonFlotante;

   
    private bool mouseEstaEncima = false;

   
    private void OnMouseEnter()
    {
        if (botonFlotante != null)
        {
            botonFlotante.SetActive(true);
            mouseEstaEncima = true; 
        }
    }

    
    private void OnMouseExit()
    {
        if (botonFlotante != null)
        {
            botonFlotante.SetActive(false);
            mouseEstaEncima = false;
        }
    }

    
    private void Update()
    {
        // si el mouse está encima del objeto y apretas la q abre inspeccion de objetos
        if (mouseEstaEncima && Input.GetKeyDown(KeyCode.Q))
        {
            EjecutarInspeccion();
        }
    }

    
    private void EjecutarInspeccion()
    {
        Debug.Log("Abriendo inspección de objetos...");
        
    }
}