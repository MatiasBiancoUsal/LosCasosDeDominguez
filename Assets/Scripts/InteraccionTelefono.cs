using UnityEngine;
using System.Collections;

public class InteraccionTelefono : MonoBehaviour
{
    [Header("Configuración de UI y Animación")]
    [SerializeField] private GameObject panelParaAbrir;
    [SerializeField] private Animator animatorDelObjeto;
    [SerializeField] private string nombreAnimacion = "telefono_espera"; // Asegúrate de que coincida con tu archivo

    [Header("Tiempos de Espera Automático")]
    [SerializeField] private float segundosParaArrancarAnimacion = 5f;

    private bool laAnimacionYaEmpezo = false;
    private bool yaSeInteractuo = false;

    // Start se ejecuta automáticamente al iniciar el juego
    private void Start()
    {
        // Arranca la cuenta regresiva apenas empieza la escena
        StartCoroutine(EsperarYActivarAnimacionAutomatica());
    }

    // Update se ejecuta una vez por fotograma
    private void Update()
    {
        // REQUISITO: Solo deja presionar la 'A' si la animación ya empezó Y si no se interactuó antes
        if (laAnimacionYaEmpezo && !yaSeInteractuo)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                InteractuarConTelefono();
            }
        }
    }

    // Corrutina que corre en segundo plano al arrancar el juego
    private IEnumerator EsperarYActivarAnimacionAutomatica()
    {
        // Espera los 5 segundos configurados
        yield return new WaitForSeconds(segundosParaArrancarAnimacion);

        // Activa la animación en el Animator
        if (animatorDelObjeto != null)
        {
            animatorDelObjeto.Play(nombreAnimacion, 0, 0f);
            laAnimacionYaEmpezo = true;
            Debug.Log("El teléfono empezó a sonar/moverse. ¡Ya puedes presionar A!");
        }
    }

    private void InteractuarConTelefono()
    {
        yaSeInteractuo = true;

        // Abre el panel de la interfaz del teléfono
        if (panelParaAbrir != null)
        {
            panelParaAbrir.SetActive(true);
        }

        // Opcional: Aquí podrías detener la animación si quieres que deje de sonar al abrirlo
        // animatorDelObjeto.Play("Espera", 0, 0f); 
    }
}