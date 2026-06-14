using UnityEngine;

public class ControladorMapa : MonoBehaviour
{

    public GameObject panelMapa;
    public AudioSource reproductorSonido;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

    
    public bool mapaAbierto;

    void Start()
    {
      
        mapaAbierto = false;
        panelMapa.SetActive(false);
    }

    void Update()
    {
        
        if (Input.GetKeyDown("b"))
        {
            if (mapaAbierto == false)
            {
                
                mapaAbierto = true;
                panelMapa.SetActive(true);
                reproductorSonido.PlayOneShot(sonidoAbrir);
            }
            else
            {
               
                mapaAbierto = false;
                panelMapa.SetActive(false);
                reproductorSonido.PlayOneShot(sonidoCerrar);
            }
        }
    }
}