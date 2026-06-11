using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;


public class DialogoManager : MonoBehaviour 
{
   
    public static DialogoManager Instance;

    [Header("Componentes Visuales del Canvas")]
    public GameObject panelDialogo; 
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI cajaTexto;
    public Image imgPersonaje;


    [Header("EfectoMaquina")]

    public float velocidadTexto = 0.05f;
    private bool escribiendo = false;
    private Coroutine efectoMaquinaCoroutine;


    private DialogoSistema conversacionActual;
    private int lineaActual = 0;
    private bool ignorarInputEsteFrame = false;

    [Header("Cuando termina aquí llamamos a más cosas")]
    public UnityEvent _alTerminarDialogo;

    // Interno, maneja el estado
    bool terminoDialogo;

    // sea puede ller externo en otras scripts para saber si el dialogo ya termino
    public bool TerminoDialogo { get { return terminoDialogo; } }

    private void Awake()
    {
        Instance = this; 
        panelDialogo.SetActive(false); 
    }


    public void Update()
    {
        if (panelDialogo.activeSelf)
        {
            if (ignorarInputEsteFrame)
            {
                ignorarInputEsteFrame = false;      
                return;

            }


            if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
               
                if (escribiendo)
                {
                    StopCoroutine(efectoMaquinaCoroutine);
                    cajaTexto.text = conversacionActual.dialogos[lineaActual].dialogo;
                    escribiendo = false;
                }
                else
                {
                    SiguienteLinea();
                }

            }
        }
    }

    // funcion de arranque
    public void IniciarDialogo(DialogoSistema nuevoDialogo)
    {
        conversacionActual = nuevoDialogo;
        lineaActual = 0; 
        panelDialogo.SetActive(true); 
        ignorarInputEsteFrame = true;
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


            if (efectoMaquinaCoroutine != null)
            {
                StopCoroutine(efectoMaquinaCoroutine);
            }

            efectoMaquinaCoroutine = StartCoroutine(EscribirTexto(fila.dialogo));

        }
        else
        {
            
            CerrarDialogo();
        }
    }

   
    private IEnumerator EscribirTexto (string textoCompleto)
    {
        escribiendo = true;
        cajaTexto.text = "";

        foreach (char letra in textoCompleto)
        {
            cajaTexto.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }
        escribiendo = false;
    }

    public void SiguienteLinea()
    {
        lineaActual++; 
        MostrarLinea(); 
    }

    public void CerrarDialogo()
    {
        panelDialogo.SetActive(false);
        _alTerminarDialogo.Invoke();
        terminoDialogo = false;
    }
}