using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Configuración del Minijuego")]
    [SerializeField] private float tiempoLimite = 25f;
    [SerializeField] private int totalPistasRequeridas = 3;
    [SerializeField] private string nombreEscenaHabitacion = "HabitaciónPrincipal_Nivel9";

    [Header("Recompensa por Completar")]
    [Tooltip("Flag que se le entregará al jugador al juntar todas las pistas del armario.")]
    [SerializeField] private GameFlag flagMinijuegoArmarioCompletado;

    [Header("Panel de Instrucciones")]
    [Tooltip("El panel de UI de instrucciones que se cerrará al presionar 'X'.")]
    [SerializeField] private GameObject panelInstrucciones;

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
            if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
            OtorgarFlagCompletado();
            DesactivarModoJuego();
            if (controladorRevision != null)
                controladorRevision.IniciarRevision();
        }
    }

    private void Update()
    {
        if (!juegoActivo && !TodasLasPistasObtenidas())
        {
            bool presionoX = false;

            if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
            {
                presionoX = true;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                presionoX = true;
            }

            if (presionoX)
            {
                CerrarInstruccionesYEmpezar();
            }

            return;
        }

        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        ActualizarTextoTiempo();

        if (tiempoRestante <= 0)
        {
            TiempoAgotado();
        }
    }

    public void CerrarInstruccionesYEmpezar()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }

        IniciarMinijuego();
    }

    public void IniciarMinijuego()
    {
        if (!TodasLasPistasObtenidas())
        {
            juegoActivo = true;
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
            OtorgarFlagCompletado();
            DesactivarModoJuego();

            if (controladorRevision != null)
            {
                controladorRevision.IniciarRevision();
            }
        }
    }

    private void OtorgarFlagCompletado()
    {
        if (flagMinijuegoArmarioCompletado != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(flagMinijuegoArmarioCompletado);
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