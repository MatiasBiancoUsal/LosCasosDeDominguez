using UnityEngine;
using System.Collections;

public class InteraccionTelefono : MonoBehaviour
{
    [Header("Configuración de UI y Animación")]
    [SerializeField] private GameObject panelParaAbrir;
    [SerializeField] private Animator animatorDelObjeto;
    [SerializeField] private string nombreAnimacion = "New Animation";

    [Header("Tiempos de Espera (Modifícalos en el Inspector)")]
    [SerializeField] private float segundosEsperaAnimacion = 3.0f;

    private bool yaSeInteractuo = false;

    // Update se ejecuta una vez por fotograma
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            IntentarInteraccion();
        }
    }

    private void IntentarInteraccion()
    {
       
        if (yaSeInteractuo) return;
        yaSeInteractuo = true;

        if (panelParaAbrir != null)
        {
            panelParaAbrir.SetActive(true);
        }

        StartCoroutine(EsperarYAnimarFirme());
    }

    private IEnumerator EsperarYAnimarFirme()
    {
        yield return new WaitForSeconds(segundosEsperaAnimacion);

        if (animatorDelObjeto != null)
        {
            animatorDelObjeto.Play(nombreAnimacion, 0, 0f);
        }
    }
}