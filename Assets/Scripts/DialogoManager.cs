using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogoManager : MonoBehaviour 
{
   
    public static DialogoManager Instance;

    [Header("Componentes Visuales del Canvas")]
    public GameObject panelDialogo; 
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI cajaTexto;
    public Image imgPersonaje;

    
    private DialogoSistema conversacionActual;
    private int lineaActual = 0;

    private void Awake()
    {
        Instance = this; 
        panelDialogo.SetActive(false); 
    }


    public void Update()
    {
        if (panelDialogo.activeSelf)
        {
             if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                SiguienteLinea();
            }
        }
    }

    // funcion de arranque
    public void IniciarDialogo(DialogoSistema nuevoDialogo)
    {
        conversacionActual = nuevoDialogo;
        lineaActual = 0; 
        panelDialogo.SetActive(true); 
        MostrarLinea();
    }

    
    public void MostrarLinea() //q vaya mostrando los dialogos, y cuando no haya mas, que lo cierre 
    {
      
        if (lineaActual < conversacionActual.dialogos.Length)
        {
           
            Dialog fila = conversacionActual.dialogos[lineaActual];

            
            textoNombre.text = fila.nombre;
            cajaTexto.text = fila.dialogo;

            if (fila.personaje != null)
            {
                imgPersonaje.sprite = fila.personaje;
            }
        }
        else
        {
            
            CerrarDialogo();
        }
    }

   
    public void SiguienteLinea()
    {
        lineaActual++; 
        MostrarLinea(); 
    }

    public void CerrarDialogo()
    {
        panelDialogo.SetActive(false);
    }
}