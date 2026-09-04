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

    [Header("Referencia al Controlador de Revisión")]
    [SerializeField] private ControladorRevisionFinal controladorRevision;

    private float tiempoRestante;
    private bool juegoActivo = true;

    private void Start()
    {
        tiempoRestante = tiempoLimite;

        // Si ya juntó las 3 banderas anteriormente
        if (TodasLasPistasObtenidas())
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
            if (!GameStateManager.Instance.TieneBandera(flag))
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

    private void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        if (textoTiempo != null)
            textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString() + "s";

        if (tiempoRestante <= 0)
            TiempoAgotado();
    }

    public void PistaRecolectada(GameObject objetoPista)
    {
        if (!juegoActivo) return;

        objetoPista.SetActive(false);
        VerificarProgreso();
    }

    private void VerificarProgreso()
    {
        int encontradas = 0;

        if (GameStateManager.Instance != null && banderasRequeridas != null)
        {
            foreach (GameFlag flag in banderasRequeridas)
            {
                if (GameStateManager.Instance.TieneBandera(flag))
                    encontradas++;
            }
        }

        if (textoContadorPistas != null)
            textoContadorPistas.text = encontradas + " / " + totalPistasRequeridas;

        if (encontradas >= totalPistasRequeridas)
        {
            DesactivarModoJuego();

            if (controladorRevision != null)
            {
                controladorRevision.IniciarRevision();
            }
        }
    }

    private void TiempoAgotado() => VolverAHabitacion();

    public void VolverAHabitacion()
    {
        ArmarioInteractuable.ActivarCooldown(10f);
        SceneManager.LoadScene(nombreEscenaHabitacion);
    }
}