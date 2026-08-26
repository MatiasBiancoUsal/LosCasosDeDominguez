using UnityEngine;
using TMPro; // Asegurate de tener esto si usás TextMeshPro

public class BitacoraUI : MonoBehaviour
{
    [Header("Paneles principales")]
    [SerializeField] private GameObject panelPersonajes;
    [SerializeField] private GameObject panelFichaPersonaje;

    [Header("UI de la Ficha")]
    [SerializeField] private UnityEngine.UI.Image fotoFicha;
    [SerializeField] private TextMeshProUGUI textoInformacion;

    private bool estaAbierta = false;

    private void Start()
    {
        // Empieza oculta al iniciar el juego
        CerrarBitacora();
    }

    private void Update()
    {
        // Presionar W para abrir/cerrar
        if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleBitacora();
        }
    }

    public void ToggleBitacora()
    {
        estaAbierta = !estaAbierta;

        if (estaAbierta)
        {
            MostrarGaleria();
        }
        else
        {
            CerrarBitacora();
        }
    }

    public void MostrarGaleria()
    {
        panelPersonajes.SetActive(true);
        panelFichaPersonaje.SetActive(false);
    }

    public void CerrarBitacora()
    {
        panelPersonajes.SetActive(false);
        panelFichaPersonaje.SetActive(false);
        estaAbierta = false;
    }

    // Este es el método que llamás desde el botón de la foto
    public void AbrirFicha(SuspectData sospechoso)
    {
        panelPersonajes.SetActive(false);
        panelFichaPersonaje.SetActive(true);

        if (sospechoso == null)
        {
            Debug.LogError("[Bitacora] Se intentó abrir una ficha pero el SuspectData pasado es NULL.");
            return;
        }

        // Cargar foto
        if (fotoFicha != null)
        {
            fotoFicha.sprite = sospechoso.portrait;
        }

        // Cargar y escribir el texto de la información
        CargarInformacion(sospechoso);
    }

    // Método que procesa las entradas y escribe en el texto
    private void CargarInformacion(SuspectData sospechoso)
    {
        textoInformacion.text = ""; // Limpiamos el texto previo

        if (sospechoso.notebookEntries == null || sospechoso.notebookEntries.Count == 0)
        {
            Debug.LogWarning($"[Bitacora] {sospechoso.name} no tiene entradas en notebookEntries.");
            return;
        }

        foreach (var entry in sospechoso.notebookEntries)
        {
            // Si no requiere bandera (flag null) O si el jugador ya la consiguió
            if (entry.flag == null || GameStateManager.Instance.TieneBandera(entry.flag))
            {
                textoInformacion.text += "• " + entry.texto + "\n\n";
            }
        }
    }
}