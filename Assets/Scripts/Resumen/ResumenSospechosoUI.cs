using UnityEngine;
using UnityEngine.UI;

public class ResumenSospechosoUI : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private SuspectData datosSospechoso;

    [Header("UI del Botón")]
    [SerializeField] private Image imagenPersonaje;
    [SerializeField] private Color colorDescartado = new Color(0.3f, 0.3f, 0.3f, 1f); 
    [SerializeField] private Color colorNormal = Color.white;

    private Button boton;
    private bool estaDescartado = false;
    public bool EstaDescartado => estaDescartado;

    public SuspectData DatosSospechoso => datosSospechoso;

    private void Awake()
    {
        boton = GetComponent<Button>();
        if (imagenPersonaje == null) imagenPersonaje = GetComponent<Image>();

        if (boton != null)
        {
            boton.onClick.AddListener(AlHacerClick);
        }
    }

    private void Start()
    {
        ActualizarVisual();
    }

    private void AlHacerClick()
    {
        if (ResumenManager.Instance != null && datosSospechoso != null)
        {
            ResumenManager.Instance.AbrirConclusiones(datosSospechoso, this);
        }
    }

    public void Descartar()
    {
        estaDescartado = true;
        ActualizarVisual();
    }

    private void ActualizarVisual()
    {
        if (imagenPersonaje != null)
        {
            imagenPersonaje.color = estaDescartado ? colorDescartado : colorNormal;
        }
    }
}