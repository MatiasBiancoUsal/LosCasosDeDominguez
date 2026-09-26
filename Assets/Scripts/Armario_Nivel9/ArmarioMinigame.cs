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
        CargarPistasPrevias();

        ActualizarTextoTiempo();

        ActualizarTextoContador();

        if (TodasLasPistasObtenidas())
        {
            DesactivarModoJuego();
            if (controladorRevision != null)
                controladorRevision.IniciarRevision();
        }
    }

    private void CargarPistasPrevias()
    {
        pistasEncontradasActuales = 0;

        if (GameStateManager.Instance != null && banderasRequeridas != null)
        {
            foreach (GameFlag flag in banderasRequeridas)
            {
                if (flag != null && GameStateManager.Instance.TieneBandera(flag))
                {
                    pistasEncontradasActuales++;
                }
            }
        }
    }

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

        tiempoRestante -= Time.deltaTime;
        ActualizarTextoTiempo();

        if (tiempoRestante <= 0)
        {
            TiempoAgotado();
        }
    }

    private void ActualizarTextoTiempo()
    {
        if (textoTiempo != null)
        {
            int segundos = Mathf.Max(0, Mathf.CeilToInt(tiempoRestante));
            textoTiempo.text = segundos.ToString() + "s";
        }
    }

    public void ActualizarTextoContador()
    {
        if (textoContadorPistas != null)
        {
            textoContadorPistas.text = $"{pistasEncontradasActuales} / {totalPistasRequeridas}";
        }
    }

    public void PistaRecolectada(GameObject objetoPista)
    {
        objetoPista.SetActive(false);

        pistasEncontradasActuales++;
        ActualizarTextoContador();

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
        return pistasEncontradasActuales >= totalPistasRequeridas;
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