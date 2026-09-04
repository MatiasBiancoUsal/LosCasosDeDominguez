using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorRevisionFinal : MonoBehaviour
{
    [Header("Datos de la Secuencia")]
    [SerializeField] private PistasScriptable[] listaObjetos;
    [SerializeField] private string nombreEscenaHabitacion = "HabitaciónPrincipal_Nivel9";

    [Header("UI Pantalla de Revisión")]
    [SerializeField] private GameObject panelRevision;
    [SerializeField] private Image imagenFoto;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoInfo;
    [SerializeField] private Button botonSiguiente;

    [Header("UI Salida (Se activa al terminar o abrir la revisión)")]
    [SerializeField] private GameObject botonVolverHabitacion;

    private int indiceActual = 0;

    private void Start()
    {
        // Al arrancar dejamos los paneles ocultos por defecto
        if (panelRevision != null)
            panelRevision.SetActive(false);

        if (botonVolverHabitacion != null)
            botonVolverHabitacion.SetActive(false);

        ConfigurarBotonSiguiente();
    }

    private void ConfigurarBotonSiguiente()
    {
        if (botonSiguiente != null)
        {
            botonSiguiente.onClick.RemoveAllListeners();
            botonSiguiente.onClick.AddListener(AvanzarSecuencia);
        }
    }

    public void IniciarRevision()
    {
        if (listaObjetos == null || listaObjetos.Length == 0) return;

        indiceActual = 0;
        ConfigurarBotonSiguiente();

        if (panelRevision != null)
            panelRevision.SetActive(true);

        if (botonVolverHabitacion != null)
            botonVolverHabitacion.SetActive(true);

        MostrarObjetoActual();
    }

    private void MostrarObjetoActual()
    {
        if (indiceActual < listaObjetos.Length)
        {
            PistasScriptable objetoActual = listaObjetos[indiceActual];

            if (objetoActual != null)
            {
                if (imagenFoto != null)
                    imagenFoto.sprite = objetoActual.imagenObjeto;

                if (textoNombre != null)
                    textoNombre.text = objetoActual.nombreObjeto;

                if (textoInfo != null)
                    textoInfo.text = objetoActual.descripcionObjeto;
            }
        }
    }

    public void AvanzarSecuencia()
    {
        if (panelRevision != null && !panelRevision.activeSelf) return;

        indiceActual++;

        if (indiceActual < listaObjetos.Length)
        {
            MostrarObjetoActual();
        }
        else
        {
            // Oculta el panel con los textos pero deja el botón de Volver disponible
            if (panelRevision != null)
                panelRevision.SetActive(false);
        }
    }

    public void VolverAHabitacion()
    {
        SceneManager.LoadScene(nombreEscenaHabitacion);
    }
}