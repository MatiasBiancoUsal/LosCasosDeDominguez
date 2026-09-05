using UnityEngine;
using System.Collections;

public class PanelNotificacionUI3 : MonoBehaviour
{
    [Header("Pistas necesarias para activar el cartel")]
    public PistasScriptable pista1;
    public PistasScriptable pista2;
    public PistasScriptable pista3;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Tiempo que permanece visible")]
    public float duracion = 3f;

    private Coroutine rutinaPanel;

    private string ClaveNotificacion
    {
        get
        {
            if (pista1 == null || pista2 == null || pista3 == null)
                return "";

            return "NotificacionMostrada_3Pistas_" +
                   pista1.name + "_" +
                   pista2.name + "_" +
                   pista3.name;
        }
    }

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        // Comprobamos por si las 3 pistas
        // ya habían sido obtenidas antes de cargar esta escena.
        ComprobarPistas();
    }

    private void ComprobarPistas()
    {
        if (pista1 == null || pista2 == null || pista3 == null)
        {
            Debug.LogWarning("PanelNotificacionUI3: Faltan asignar una o más pistas.");
            return;
        }

        // Si el cartel ya apareció anteriormente, no hacemos nada.
        if (NotificacionYaMostrada())
            return;

        bool tienePista1 = TienePista(pista1);
        bool tienePista2 = TienePista(pista2);
        bool tienePista3 = TienePista(pista3);

        Debug.Log(
            "Pistas: " +
            tienePista1 + " / " +
            tienePista2 + " / " +
            tienePista3
        );

        // Las 3 tienen que estar obtenidas.
        if (tienePista1 && tienePista2 && tienePista3)
        {
            MostrarCartel();
        }
    }

    private bool TienePista(PistasScriptable pista)
    {
        if (pista == null)
            return false;

        string clave = "PistaObtenida_" + pista.name;

        return PlayerPrefs.GetInt(clave, 0) == 1;
    }

    public void RegistrarPistaObtenida(PistasScriptable pista)
    {
        if (pista == null)
            return;

        string clave = "PistaObtenida_" + pista.name;

        PlayerPrefs.SetInt(clave, 1);
        PlayerPrefs.Save();

        Debug.Log("Pista registrada como obtenida: " + pista.name);

        // Después de registrar la pista,
        // comprobamos si ahora están las 3.
        ComprobarPistas();
    }

    private bool NotificacionYaMostrada()
    {
        if (string.IsNullOrEmpty(ClaveNotificacion))
            return false;

        return PlayerPrefs.GetInt(ClaveNotificacion, 0) == 1;
    }

    private void MostrarCartel()
    {
        if (panel == null)
            return;

        // Guardamos que esta combinación de 3 pistas
        // ya activó el cartel.
        PlayerPrefs.SetInt(ClaveNotificacion, 1);
        PlayerPrefs.Save();

        if (rutinaPanel != null)
        {
            StopCoroutine(rutinaPanel);
        }

        rutinaPanel = StartCoroutine(MostrarPanel());
    }

    private IEnumerator MostrarPanel()
    {
        panel.SetActive(true);

        yield return new WaitForSeconds(duracion);

        panel.SetActive(false);

        rutinaPanel = null;
    }
}