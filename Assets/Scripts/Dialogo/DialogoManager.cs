using System; // OBLIGATORIO: Necesario para los eventos C# (Action)
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance { get; private set; }

    [Header("Componentes Visuales del Canvas")]
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI cajaTexto;
    [SerializeField] private Image imgPersonaje;

    [Header("Efecto Maquina")]
    [SerializeField] private float velocidadTexto = 0.05f;

    // Evento C# al que se suscribirá ActivarDialogo para guardar la bandera
    public event Action OnDialogoFinalizado;

    private bool escribiendo = false;
    private Coroutine efectoMaquinaCoroutine;
    private DialogoSistema conversacionActual;
    private int lineaActual = 0;
    private bool ignorarInputEsteFrame = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }
    }

    public void Update()
    {
        if (panelDialogo == null || !panelDialogo.activeSelf)
            return;

        if (ignorarInputEsteFrame)
        {
            ignorarInputEsteFrame = false;
            return;
        }

        // Avanzar o completar texto con la tecla F
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (escribiendo)
            {
                if (efectoMaquinaCoroutine != null)
                    StopCoroutine(efectoMaquinaCoroutine);

                if (cajaTexto != null && conversacionActual != null)
                    cajaTexto.text = conversacionActual.dialogos[lineaActual].dialogo;

                escribiendo = false;
            }
            else
            {
                SiguienteLinea();
            }
        }
    }

    public void IniciarDialogo(DialogoSistema nuevoDialogo)
    {
        if (nuevoDialogo == null || nuevoDialogo.dialogos == null || nuevoDialogo.dialogos.Length == 0)
        {
            Debug.LogWarning("[DialogoManager] Intentando iniciar un diálogo nulo o sin líneas.");
            return;
        }

        conversacionActual = nuevoDialogo;
        lineaActual = 0;

        if (panelDialogo != null)
            panelDialogo.SetActive(true);

        ignorarInputEsteFrame = true;
        MostrarLinea();
    }

    public void MostrarLinea()
    {
        if (conversacionActual == null) return;

        if (lineaActual < conversacionActual.dialogos.Length)
        {
            Dialog fila = conversacionActual.dialogos[lineaActual];

            if (textoNombre != null)
                textoNombre.text = fila.nombre;

            if (imgPersonaje != null && fila.personaje != null)
            {
                imgPersonaje.sprite = fila.personaje;
            }

            if (efectoMaquinaCoroutine != null)
            {
                StopCoroutine(efectoMaquinaCoroutine);
            }

            efectoMaquinaCoroutine = StartCoroutine(EscribirTexto(fila.dialogo));
        }
        else
        {
            CerrarDialogo();
        }
    }

    private IEnumerator EscribirTexto(string textoCompleto)
    {
        escribiendo = true;
        if (cajaTexto != null) cajaTexto.text = "";

        foreach (char letra in textoCompleto)
        {
            if (cajaTexto != null) cajaTexto.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }

    public void SiguienteLinea()
    {
        lineaActual++;
        MostrarLinea();
    }

    public void CerrarDialogo()
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }

        // Emite la señal C# indicando que el diálogo se cerró
        OnDialogoFinalizado?.Invoke();
    }
}