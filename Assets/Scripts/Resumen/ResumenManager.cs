using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResumenManager : MonoBehaviour
{
    public static ResumenManager Instance { get; private set; }

    [Header("Paneles Principales")]
    [SerializeField] private GameObject panelGaleria;
    [SerializeField] private GameObject panelConclusiones;

    [Header("UI de la Ficha / Conclusiones")]
    [SerializeField] private Image fotoSospechoso;
    [SerializeField] private TMP_Text nombreSospechoso;
    [SerializeField] private TMP_Text textoConclusiones;

    [Space(5)]
    [SerializeField] private Button botonEliminarSospechoso;
    [SerializeField] private TMP_Text textoBotonEliminar;

    [SerializeField] private Button botonVolverAGaleria;

    [Header("Control de Sospechosos")]
    [Tooltip("Lista de todos los botones de sospechosos en la galería.")]
    [SerializeField] private List<ResumenSospechosoUI> listaSospechososUI = new List<ResumenSospechosoUI>();

    [Header("Sospechosos Protegidos / Finalistas")]
    [Tooltip("Arrastra aquí los assets (SuspectData) de los 5 sospechosos que NO se pueden eliminar.")]
    [SerializeField] private List<SuspectData> sospechososFinalistas = new List<SuspectData>();

    [Header("Botón para Avanzar (Quedan 5)")]
    [Tooltip("Botón flotante en la galería que se activa cuando quedan 5 sospechosos o menos.")]
    [SerializeField] private Button botonConfirmar;

    [Header("Panel Aviso Final")]
    [Tooltip("El panel con el script EfectoTipeoUI.")]
    [SerializeField] private GameObject panelAvisoCincoSospechosos;

    private ResumenSospechosoUI sospechosoActualUI;
    private bool esSospechosoFinalista = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (botonEliminarSospechoso != null)
            botonEliminarSospechoso.onClick.AddListener(ProcesarAccionSospechoso);

        if (botonVolverAGaleria != null)
            botonVolverAGaleria.onClick.AddListener(MostrarGaleria);

        if (botonConfirmar != null)
        {
            botonConfirmar.onClick.AddListener(MostrarCartelAviso);
            botonConfirmar.gameObject.SetActive(false);
        }

        if (panelAvisoCincoSospechosos != null)
            panelAvisoCincoSospechosos.SetActive(false);
    }

    private void Start()
    {
        MostrarGaleria();
    }

    public void MostrarGaleria()
    {
        if (panelGaleria != null) panelGaleria.SetActive(true);
        if (panelConclusiones != null) panelConclusiones.SetActive(false);
        if (panelAvisoCincoSospechosos != null) panelAvisoCincoSospechosos.SetActive(false);

        sospechosoActualUI = null;
        VerificarSospechososRestantes();
    }

    public void AbrirConclusiones(SuspectData sospechoso, ResumenSospechosoUI UIReferencia)
    {
        if (sospechoso == null) return;

        sospechosoActualUI = UIReferencia;

        // Comprobamos directamente si el asset que abrimos está contenido en la lista de finalistas
        esSospechosoFinalista = sospechososFinalistas.Contains(sospechoso);

        // Cambiamos el texto del botón según la lista
        if (textoBotonEliminar != null)
        {
            textoBotonEliminar.text = esSospechosoFinalista ? "Seguir Investigando" : "Eliminar Sospechoso";
        }

        if (nombreSospechoso != null) nombreSospechoso.text = sospechoso.suspectName;
        if (fotoSospechoso != null) fotoSospechoso.sprite = sospechoso.portrait;

        if (textoConclusiones != null)
        {
            textoConclusiones.text = "";
            if (sospechoso.notebookEntries != null)
            {
                foreach (var entry in sospechoso.notebookEntries)
                {
                    if (entry.flag == null || (GameStateManager.Instance != null && GameStateManager.Instance.TieneBandera(entry.flag)))
                    {
                        textoConclusiones.text += "• " + entry.texto + "\n\n";
                    }
                }
            }
        }

        if (panelGaleria != null) panelGaleria.SetActive(false);
        if (panelConclusiones != null) panelConclusiones.SetActive(true);
    }

    public void ProcesarAccionSospechoso()
    {
        // Solo descartamos (pintamos en gris) si NO es finalista
        if (!esSospechosoFinalista && sospechosoActualUI != null)
        {
            sospechosoActualUI.Descartar();
        }

        MostrarGaleria();
    }

    private void VerificarSospechososRestantes()
    {
        int activos = 0;

        foreach (var sospechosoUI in listaSospechososUI)
        {
            if (sospechosoUI != null && !sospechosoUI.EstaDescartado)
            {
                activos++;
            }
        }

        if (botonConfirmar != null)
        {
            botonConfirmar.gameObject.SetActive(activos <= 5);
        }
    }

    public void MostrarCartelAviso()
    {
        if (panelAvisoCincoSospechosos != null)
        {
            panelAvisoCincoSospechosos.SetActive(true);
        }
    }
}