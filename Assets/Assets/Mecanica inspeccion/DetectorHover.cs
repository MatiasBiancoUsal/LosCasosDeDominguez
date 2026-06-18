using UnityEngine;

public class DetectorHover : MonoBehaviour
{
    public GameObject botonFlotante;

    private bool mouseEstaEncima = false;

    // Permite que otros scripts sepan si el mouse está sobre este objeto
    public bool MouseEstaEncima => mouseEstaEncima;


    private void OnMouseEnter()

    {
        Debug.Log("ENTER DETECTOR HOVER ELISA");
        if (botonFlotante != null)
        {
            botonFlotante.SetActive(true);
        }

        mouseEstaEncima = true;
    }

    private void OnMouseExit()
    {
        if (botonFlotante != null)
        {
            botonFlotante.SetActive(false);
        }

        mouseEstaEncima = false;
    }

    private void Update()
    {
        // Si el mouse está encima y apretás Q
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