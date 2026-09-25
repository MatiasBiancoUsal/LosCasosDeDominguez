using UnityEngine;

public class ControladorMapa : MonoBehaviour
{

    public GameObject panelMapa;
    public AudioSource reproductorSonido;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;
    public GameObject fondoNegro;


    public bool mapaAbierto;

    void Start()
    {
      
        mapaAbierto = false;
        panelMapa.SetActive(false);

        if (fondoNegro != null)
        {
            fondoNegro.SetActive(false);
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown("b"))
        {
            if (mapaAbierto == false)
            {
                
                mapaAbierto = true;
                panelMapa.SetActive(true);

                if (fondoNegro != null)
                    fondoNegro.SetActive(true);

                reproductorSonido.PlayOneShot(sonidoAbrir);
            }
            else
            {
               
                mapaAbierto = false;
                panelMapa.SetActive(false);

                if (fondoNegro != null)
                    fondoNegro.SetActive(false);

                reproductorSonido.PlayOneShot(sonidoCerrar);
            }
        }
    }
}