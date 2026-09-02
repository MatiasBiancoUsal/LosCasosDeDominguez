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
    [SerializeField] private Button botonEliminarSospechoso;
    [SerializeField] private Button botonVolverAGaleria;

    [Header("Control de Sospechosos")]
    [Tooltip("Lista de todos los botones de sospechosos en la galería.")]
    [SerializeField] private List<ResumenSospechosoUI> listaSospechososUI = new List<ResumenSospechosoUI>();

    [Header("Botón para Avanzar (Quedan 4)")]
    [Tooltip("Botón flotante en la galería que se activa cuando quedan 4 sospechosos o menos.")]
    [SerializeField] private Button botonConfirmarCuatro;

    [Header("Panel Aviso Final")]
    [Tooltip("El panel con el script EfectoTipeoUI.")]
    [SerializeField] private GameObject panelAvisoCuatroSospechosos;

    private ResumenSospechosoUI sospechosoActualUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (botonEliminarSospechoso != null)
            botonEliminarSospechoso.onClick.AddListener(EliminarSospechosoActual);

        if (botonVolverAGaleria != null)
            botonVolverAGaleria.onClick.AddListener(MostrarGaleria);

        if (botonConfirmarCuatro != null)
        {
            botonConfirmarCuatro.onClick.AddListener(MostrarCartelAviso);
            botonConfirmarCuatro.gameObject.SetActive(false); 
        }

        if (panelAvisoCuatroSospechosos != null)
            panelAvisoCuatroSospechosos.SetActive(false);
    }

    private void Start()
    {
        MostrarGaleria();
    }

    public void MostrarGaleria()
    {
        if (panelGaleria != null) panelGaleria.SetActive(true);
        if (panelConclusiones != null) panelConclusiones.SetActive(false);
        if (panelAvisoCuatroSospechosos != null) panelAvisoCuatroSospechosos.SetActive(false);

        sospechosoActualUI = null;

        VerificarSospechososRestantes();
    }

    public void AbrirConclusiones(SuspectData sospechoso, ResumenSospechosoUI UIReferencia)
    {
        if (sospechoso == null) return;

        sospechosoActualUI = UIReferencia;

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

    public void EliminarSospechosoActual()
    {
        if (sospechosoActualUI != null)
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

        if (botonConfirmarCuatro != null)
        {
            botonConfirmarCuatro.gameObject.SetActive(activos <= 4);
        }
    }

    public void MostrarCartelAviso()
    {
        if (panelAvisoCuatroSospechosos != null)
        {
            panelAvisoCuatroSospechosos.SetActive(true);
        }
    }
}