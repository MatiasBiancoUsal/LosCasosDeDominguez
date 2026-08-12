using UnityEngine;
using UnityEngine.InputSystem;

public class ActivarDialogo : MonoBehaviour
{
    [System.Serializable]
    public struct CondicionDialogo
    {
        public string clavePlayerPref;
        public DialogoSistema dialogo; 
        public string claveObjetoRequerido; 
    }

    [Header("Lista de diálogos priorizados (de arriba a abajo)")]
    [SerializeField] private CondicionDialogo[] dialogosPosibles;

    [Header("Diálogo por defecto (cuando se agotaron los demás)")]
    [SerializeField] private DialogoSistema dialogoPorDefecto;

    private DetectorHover hover;
    private CondicionDialogo? dialogoActual;

    private void Awake()
    {
        hover = GetComponent<DetectorHover>();
    }

    private void Update()
    {

        if (hover != null && hover.MouseEstaEncima && Keyboard.current.iKey.wasPressedThisFrame)
        {
            Debug.Log("I DETECTADA EN ACTIVAR DIALOGO: " + gameObject.name);

            EvaluarYIniciarDialogo();
        }
    }

    private void EvaluarYIniciarDialogo()
    {
        DialogoSistema dialogoAProcesar = null;
        dialogoActual = null;

        foreach (var cond in dialogosPosibles)
        {
            // 1. Si ya se guardó en PlayerPrefs que este diálogo ocurrió (valor 1), se salta al siguiente
            if (PlayerPrefs.GetInt(cond.clavePlayerPref, 0) == 1)
            {
                continue;
            }

            // 2. Si requiere un objeto, verifica si esa clave existe en PlayerPrefs con valor 1
            if (!string.IsNullOrEmpty(cond.claveObjetoRequerido))
            {
                if (PlayerPrefs.GetInt(cond.claveObjetoRequerido, 0) == 0)
                {
                    continue; // No tiene el objeto necesario aún
                }
            }

            // Si pasa las condiciones, seleccionamos este diálogo
            dialogoAProcesar = cond.dialogo;
            dialogoActual = cond;
            break;
        }

        // Si ya vio todos los diálogos o no cumple las condiciones, usa el diálogo por defecto
        if (dialogoAProcesar == null)
        {
            dialogoAProcesar = dialogoPorDefecto;
        }

        Debug.Log("DIALOGO SELECCIONADO: " +
        (dialogoAProcesar != null ? dialogoAProcesar.name : "NULL"));

        if (dialogoAProcesar != null)
        {
            // Escuchar el evento de cuando termina el diálogo
            DialogoManager.Instance._alTerminarDialogo.RemoveListener(OnDialogoFinalizado);
            DialogoManager.Instance._alTerminarDialogo.AddListener(OnDialogoFinalizado);

            DialogoManager.Instance.IniciarDialogo(dialogoAProcesar);
        }
    }

    private void OnDialogoFinalizado()
    {
        // Al terminar, si provenía de la lista, lo guardamos en PlayerPrefs como completado (1)
        if (dialogoActual.HasValue)
        {
            PlayerPrefs.SetInt(dialogoActual.Value.clavePlayerPref, 1);
            PlayerPrefs.Save();
        }

        DialogoManager.Instance._alTerminarDialogo.RemoveListener(OnDialogoFinalizado);
    }
}