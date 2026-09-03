using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ArmarioMinigame : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoLimite = 25f;
    [SerializeField] private int totalPistasRequeridas = 3;

    [Header("Banderas de Pistas en este Minijuego")]
    [SerializeField] private List<GameFlag> banderasRequeridas;

    [Header("Referencias UI Escena Minijuego")]
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [SerializeField] private TextMeshProUGUI textoContadorPistas;
    [SerializeField] private GameObject panelNotificacionFinal;

    private float tiempoRestante;
    private bool juegoActivo = true;
    private int pistasConseguidasCount = 0;

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        if (panelNotificacionFinal != null) panelNotificacionFinal.SetActive(false);

        VerificarProgreso();
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

    private void VerificarProgreso()
    {
        pistasConseguidasCount = 0;

        if (GameStateManager.Instance != null && banderasRequeridas != null)
        {
            foreach (GameFlag flag in banderasRequeridas)
            {
                if (GameStateManager.Instance.TieneBandera(flag))
                {
                    pistasConseguidasCount++;
                }
            }
        }

        ActualizarTextoContador();

        if (pistasConseguidasCount >= totalPistasRequeridas)
            GanarMinijuego();
    }

    public void PistaRecolectada(GameObject objetoPista)
    {
        if (!juegoActivo) return;

        objetoPista.SetActive(false);

        // Volvemos a contar desde GameStateManager para asegurar sincronización real
        VerificarProgreso();
    }

    private void ActualizarTextoContador()
    {
        if (textoContadorPistas != null)
            textoContadorPistas.text = pistasConseguidasCount + " / " + totalPistasRequeridas;
    }

    private void GanarMinijuego()
    {
        juegoActivo = false;
        if (panelNotificacionFinal != null) panelNotificacionFinal.SetActive(true);
    }

    private void TiempoAgotado() => CerrarEscenaMinijuego();

    public void CerrarEscenaMinijuego()
    {
        ArmarioInteractuable.ActivarCooldown(10f);
        SceneManager.LoadScene("HabitaciónPrincipal_Nivel9");
    }
}