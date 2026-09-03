using System.Collections;
using UnityEngine;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject canvasArmarioUI;
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [SerializeField] private GameObject botonAbrirArmario; // El indicador visual que dice "Presiona Q"

    [Header("Configuración de Tiempos")]
    [SerializeField] private float tiempoLimite = 12f;
    [SerializeField] private float tiempoCooldown = 30f;

    private int pistasEncontradas = 0;
    private float tiempoRestante;
    private bool juegoActivo = false;
    private bool enCooldown = false;
    private bool mouseEncima = false;

    private Collider2D miCollider;
    private Coroutine corrutinaTiempo;

    private void Awake()
    {
        // Obtenemos el Collider 2D del armario automáticamente
        miCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Si la cámara no existe o la UI principal está activa, evitamos el cálculo
        if (Camera.main == null) return;

        // Convertimos la posición del ratón en la pantalla a coordenadas del mundo 2D
        Vector2 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Detectar si el ratón está exactamente sobre el BoxCollider2D del armario
        if (miCollider != null && miCollider.OverlapPoint(posicionRaton))
        {
            // Evento: El cursor ENTRÓ al área del armario
            if (!mouseEncima)
            {
                mouseEncima = true;
                if (botonAbrirArmario != null && !enCooldown && !juegoActivo)
                {
                    botonAbrirArmario.SetActive(true);
                }
            }

            // Evento: Presiona la tecla Q para iniciar el minijuego
            if (Input.GetKeyDown(KeyCode.Q) && !juegoActivo && !enCooldown)
            {
                AbrirMinijuegoArmario();
            }
        }
        else
        {
            // Evento: El cursor SALIÓ del área del armario
            if (mouseEncima)
            {
                mouseEncima = false;
                if (botonAbrirArmario != null)
                {
                    botonAbrirArmario.SetActive(false);
                }
            }
        }
    }

    public void AbrirMinijuegoArmario()
    {
        if (enCooldown)
        {
            Debug.Log("El detective dice: 'Recién revisé ahí, necesito un momento'.");
            return;
        }

        // Reiniciar variables del juego
        pistasEncontradas = 0;
        tiempoRestante = tiempoLimite;
        juegoActivo = true;

        // Activar el Canvas del minijuego y ocultar el botón indicador de la Q
        if (canvasArmarioUI != null) canvasArmarioUI.SetActive(true);
        if (botonAbrirArmario != null) botonAbrirArmario.SetActive(false);

        corrutinaTiempo = StartCoroutine(Contrarreloj());
    }

    private IEnumerator Contrarreloj()
    {
        while (tiempoRestante > 0 && juegoActivo)
        {
            tiempoRestante -= Time.deltaTime;
            if (textoTiempo != null)
            {
                textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString() + "s";
            }
            yield return null;
        }

        if (juegoActivo && pistasEncontradas < 3)
        {
            TiempoAgotado();
        }
    }

    // Llama a esta función desde el evento OnClick() de cada objeto/pista dentro del minijuego
    public void PistaEncontrada(GameObject objetoPista)
    {
        if (!juegoActivo) return;

        objetoPista.SetActive(false);
        pistasEncontradas++;

        if (pistasEncontradas >= 3)
        {
            GanarMinijuego();
        }
    }

    private void GanarMinijuego()
    {
        juegoActivo = false;
        if (corrutinaTiempo != null) StopCoroutine(corrutinaTiempo);

        if (canvasArmarioUI != null) canvasArmarioUI.SetActive(false);
        Debug.Log("¡Pistas encontradas con éxito!");
    }

    private void TiempoAgotado()
    {
        juegoActivo = false;
        if (canvasArmarioUI != null) canvasArmarioUI.SetActive(false);

        StartCoroutine(IniciarCooldown());
    }

    private IEnumerator IniciarCooldown()
    {
        enCooldown = true;
        if (botonAbrirArmario != null) botonAbrirArmario.SetActive(false);

        yield return new WaitForSeconds(tiempoCooldown);

        enCooldown = false;
    }
}