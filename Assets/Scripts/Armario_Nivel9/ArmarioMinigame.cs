using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoLimite = 25f;
    [SerializeField] private int totalPistasRequeridas = 3;
    [SerializeField] private string nombreEscenaHabitacion = "HabitaciónPrincipal_Nivel9";

    [Header("Banderas de Pistas")]
    [SerializeField] private List<GameFlag> banderasRequeridas;

    [Header("Referencias UI Escena Minijuego")]
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [SerializeField] private TextMeshProUGUI textoContadorPistas;

    [Header("Referencia al Panel de Revisión Final")]
    [SerializeField] private ControladorRevisionFinal controladorRevision;

    private float tiempoRestante;
    private bool juegoActivo = false;
    private int pistasEncontradasActuales = 0;

    private void Start()
    {
        tiempoRestante = tiempoLimite;

        // Muestra el tiempo inicial en el texto (ej: "25s")
        ActualizarTextoTiempo();

        // Muestra el contador inicial de pistas (ej: "0 / 3")
        ActualizarTextoContador();

        // Si ya tenía las 3 pistas recolectadas previamente
        if (TodasLasPistasObtenidas())
        {
            DesactivarModoJuego();
            if (controladorRevision != null)
                controladorRevision.IniciarRevision();
        }
    }

    /// <summary>
    /// Se ejecuta al cerrar el panel de explicación inicial.
    /// </summary>
    public void IniciarMinijuego()
    {
        if (!TodasLasPistasObtenidas())
        {
            juegoActivo = true;
        }
    }

    private void Update()
    {
        if (!juegoActivo) return;

        // Descuenta el tiempo segundo a segundo
        tiempoRestante -= Time.deltaTime;
        ActualizarTextoTiempo();

        // Si el tiempo se agota, regresa a la habitación
        if (tiempoRestante <= 0)
        {
            TiempoAgotado();
        }
    }

    /// <summary>
    /// Formatea y actualiza el texto del reloj en la UI.
    /// </summary>
    private void ActualizarTextoTiempo()
    {
        if (textoTiempo != null)
        {
            int segundos = Mathf.Max(0, Mathf.CeilToInt(tiempoRestante));
            textoTiempo.text = segundos.ToString() + "s";
        }
    }

    /// <summary>
    /// Actualiza el texto del contador de pistas (ej: "1 / 3").
    /// </summary>
    public void ActualizarTextoContador()
    {
        if (textoContadorPistas != null)
        {
            textoContadorPistas.text = $"{pistasEncontradasActuales} / {totalPistasRequeridas}";
        }
    }

    /// <summary>
    /// Llamado desde cada PistaArmario cuando se llena la barra con la tecla Q.
    /// </summary>
    public void PistaRecolectada(GameObject objetoPista)
    {
        objetoPista.SetActive(false);

        // Suma a la UI de pistas
        pistasEncontradasActuales++;
        ActualizarTextoContador();

        // Si completó las 3, frena el reloj y abre el panel de revisión con flechas
        if (pistasEncontradasActuales >= totalPistasRequeridas)
        {
            DesactivarModoJuego();

            if (controladorRevision != null)
            {
                controladorRevision.IniciarRevision();
            }
        }
    }

    private bool TodasLasPistasObtenidas()
    {
        if (GameStateManager.Instance == null || banderasRequeridas == null || banderasRequeridas.Count == 0)
            return false;

        foreach (GameFlag flag in banderasRequeridas)
        {
            if (flag != null && !GameStateManager.Instance.TieneBandera(flag))
                return false;
        }

        return true;
    }

    private void DesactivarModoJuego()
    {
        juegoActivo = false;

        if (textoTiempo != null)
            textoTiempo.gameObject.SetActive(false);

        if (textoContadorPistas != null)
            textoContadorPistas.text = $"{totalPistasRequeridas} / {totalPistasRequeridas}";
    }

    private void TiempoAgotado() => VolverAHabitacion();

    public void VolverAHabitacion()
    {
        ArmarioInteractuable.ActivarCooldown(10f);
        SceneManager.LoadScene(nombreEscenaHabitacion);
    }
}