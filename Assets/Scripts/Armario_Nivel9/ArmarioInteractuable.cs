using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ArmarioInteractuable : MonoBehaviour
{
    [Header("UI Indicador")]
    [SerializeField] private GameObject cartelTeclaQ;
    [SerializeField] private TextMeshProUGUI textoContadorCooldown;

    [Header("Configuración Cooldown")]
    [SerializeField] private float tiempoCooldown = 10f;

    public static bool minijuegoCompletado = false;
    public static string[] pistasObtenidas = new string[0];

    private static float tiempoDesbloqueo = 0f;

    private Collider2D miCollider;
    private bool mouseEncima = false;

    private void Awake()
    {
        miCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Camera.main == null || minijuegoCompletado) return;

        float tiempoRestante = tiempoDesbloqueo - Time.realtimeSinceStartup;
        bool enCooldown = tiempoRestante > 0;

        if (textoContadorCooldown != null)
        {
            if (enCooldown)
            {
                if (!textoContadorCooldown.gameObject.activeSelf)
                    textoContadorCooldown.gameObject.SetActive(true);

                textoContadorCooldown.text = $"Espera: {Mathf.CeilToInt(tiempoRestante)}s";
            }
            else
            {
                if (textoContadorCooldown.gameObject.activeSelf)
                    textoContadorCooldown.gameObject.SetActive(false);
            }
        }

        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (miCollider != null && miCollider.OverlapPoint(posicionRaton))
        {
            if (!mouseEncima)
            {
                mouseEncima = true;
                if (cartelTeclaQ != null && !enCooldown)
                    cartelTeclaQ.SetActive(true);
            }

            if (enCooldown && cartelTeclaQ != null && cartelTeclaQ.activeSelf)
            {
                cartelTeclaQ.SetActive(false);
            }

            if (!enCooldown && Input.GetKeyDown(KeyCode.Q))
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

    public static void ActivarCooldown(float segundos)
    {
        tiempoDesbloqueo = Time.realtimeSinceStartup + segundos;
    }

    private void CargarEscenaMinijuego()
    {
        SceneManager.LoadScene("Armario");
    }
}