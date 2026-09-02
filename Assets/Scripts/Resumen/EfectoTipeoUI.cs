using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EfectoTipeoUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text textoUI;
    [SerializeField] private Button botonAceptar;

    [Header("Navegación")]
    [SerializeField] private string nombreEscenaDestino = "EscenaInterrogatorio";

    [Header("Configuración del Texto")]
    [TextArea(5, 10)]
    [SerializeField]
    private string textoCompleto =
        "Tras descartar a los sospechosos de menor calaña, solo quedan cuatro en pie.\n\n" +
        "Es momento de apretar las clavijas y someterlos a un nuevo interrogatorio.";

    [Tooltip("Velocidad entre cada letra (en segundos).")]
    [SerializeField] private float velocidadTipeo = 0.06f;

    private Coroutine corrutinaTipeo;

    private void Awake()
    {
        if (botonAceptar != null)
        {
            botonAceptar.onClick.AddListener(CambiarDeEscena);
        }
    }

    private void OnEnable()
    {
        if (botonAceptar != null)
        {
            botonAceptar.gameObject.SetActive(false);
        }

        if (textoUI != null)
        {
            if (corrutinaTipeo != null) StopCoroutine(corrutinaTipeo);
            corrutinaTipeo = StartCoroutine(EscribirTexto());
        }
    }

    private IEnumerator EscribirTexto()
    {
        textoUI.text = "";

        foreach (char letra in textoCompleto.ToCharArray())
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(velocidadTipeo);
        }

        if (botonAceptar != null)
        {
            botonAceptar.gameObject.SetActive(true);
        }
    }

    public void CambiarDeEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogWarning("[EfectoTipeoUI] No asignaste el nombre de la escena de destino.");
        }
    }
}