
using UnityEngine;
using System.Collections;

public class InteraccionTelefono : MonoBehaviour
{
    [Header("Configuración de UI y Animación")]
    [SerializeField] private GameObject panelParaAbrir;
    [SerializeField] private Animator animatorDelObjeto;
    [SerializeField] private string nombreAnimacion = "telefono_espera";

    [Header("Flag al interactuar")]
    [Tooltip("Flag que se otorga cuando el jugador presiona A para interactuar.")]
    [SerializeField] private GameFlag flagAlInteractuar;

    [Header("Flag automática")]
    [Tooltip("Flag que se otorga automáticamente después de comenzar la animación.")]
    [SerializeField] private GameFlag flagAutomatica;

    [Tooltip("Tiempo después de comenzar la animación para otorgar la flag automática.")]
    [SerializeField] private float segundosDespuesDeLaAnimacion = 2f;

    [Header("Tiempos de Espera Automático")]
    [SerializeField] private float segundosParaArrancarAnimacion = 5f;

    public AudioClip clip;

    private bool laAnimacionYaEmpezo = false;
    private bool yaSeInteractuo = false;
    private bool flagAutomaticaYaOtorgada = false;

    private void Start()
    {
        StartCoroutine(EsperarYActivarAnimacionAutomatica());
    }

    private void Update()
    {
        if (laAnimacionYaEmpezo && !yaSeInteractuo)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                InteractuarConTelefono();
                SonidosDeUI.instance.Source.Stop();
            }
        }
    }

    private IEnumerator EsperarYActivarAnimacionAutomatica()
    {
        // Espera el tiempo configurado para comenzar la animación
        yield return new WaitForSeconds(segundosParaArrancarAnimacion);

        if (animatorDelObjeto != null)
        {
            animatorDelObjeto.Play(nombreAnimacion, 0, 0f);
            laAnimacionYaEmpezo = true;

            Debug.Log("El teléfono empezó a sonar/moverse. ¡Ya puedes presionar A!");
        }

        // Espera los segundos adicionales para otorgar la flag automática
        yield return new WaitForSeconds(segundosDespuesDeLaAnimacion);

        DarFlagAutomatica();
    }

    private void DarFlagAutomatica()
    {
        if (flagAutomaticaYaOtorgada)
            return;

        if (flagAutomatica != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(flagAutomatica);

            flagAutomaticaYaOtorgada = true;

            Debug.Log(
                "Flag automática obtenida: "
                + flagAutomatica.name
            );
        }
    }

    private void InteractuarConTelefono()
    {
        yaSeInteractuo = true;

        if (panelParaAbrir != null)
        {
            panelParaAbrir.SetActive(true);
        }

        // Esta es la flag ORIGINAL que se obtiene al presionar A
        if (flagAlInteractuar != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.GuardarBandera(flagAlInteractuar);

            Debug.Log(
                "Flag obtenida mediante interacción: "
                + flagAlInteractuar.name
            );
        }

        SonidosDeUI.instance.Play(clip);
    }

    public void Sonar()
    {
        if (!yaSeInteractuo)
        {
            SonidosDeUI.instance.Play(clip);
        }
    }
}
