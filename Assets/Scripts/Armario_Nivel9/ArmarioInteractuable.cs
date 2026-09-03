using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmarioInteractuable : MonoBehaviour
{
    [Header("UI Indicador")]
    [SerializeField] private GameObject cartelTeclaQ; 

    [Header("Configuración Escena")]
    [SerializeField] private string nombreEscenaMinijuego = "EscenaMinijuegoArmario";

    public static bool minijuegoCompletado = false;
    public static string[] pistasObtenidas = new string[0];

    private Collider2D miCollider;
    private bool mouseEncima = false;

    private void Awake()
    {
        miCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Camera.main == null || minijuegoCompletado) return;

        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (miCollider != null && miCollider.OverlapPoint(posicionRaton))
        {
            if (!mouseEncima)
            {
                mouseEncima = true;
                if (cartelTeclaQ != null) cartelTeclaQ.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (cartelTeclaQ != null) cartelTeclaQ.SetActive(false);
                CargarEscenaMinijuego();
            }
        }
        else
        {
            if (mouseEncima)
            {
                mouseEncima = false;
                if (cartelTeclaQ != null) cartelTeclaQ.SetActive(false);
            }
        }
    }

    private void CargarEscenaMinijuego()
    {
        SceneManager.LoadScene(nombreEscenaMinijuego, LoadSceneMode.Additive);
    }
}