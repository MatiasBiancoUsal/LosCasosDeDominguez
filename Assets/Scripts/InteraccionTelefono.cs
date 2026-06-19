using UnityEngine;
using System.Collections;

public class InteraccionTelefono : MonoBehaviour
{
    [Header("Configuración de UI y Animación")]
    [SerializeField] private GameObject panelParaAbrir;
    [SerializeField] private Animator animatorDelObjeto;
    [SerializeField] private string nombreAnimacion = "telefono_espera"; 

    [Header("Tiempos de Espera Automático")]
    [SerializeField] private float segundosParaArrancarAnimacion = 5f;

    private bool laAnimacionYaEmpezo = false;
    private bool yaSeInteractuo = false;

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
            }
        }
    }

    private IEnumerator EsperarYActivarAnimacionAutomatica()
    {
        yield return new WaitForSeconds(segundosParaArrancarAnimacion);

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

        if (panelParaAbrir != null)
        {
            panelParaAbrir.SetActive(true);
        }
    }
}