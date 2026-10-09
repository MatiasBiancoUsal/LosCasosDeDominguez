using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private bool canWalk = true;
    [SerializeField] private float speed = 4f;

    [Header("Efecto de Clic")]
    [SerializeField] private GameObject efectoClicPrefab;
    [SerializeField] private float tiempoDeVidaEfecto = 0.5f;

    [Header("Configuracion de capas")]
    [SerializeField] private LayerMask capaSuelo;

    private Vector3 target;
    private Camera Cam;
    private Animator animator;
    public GameObject caminataSonido;
    public bool caminando;

    void Start()
    {
        target = transform.position;
        Cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Detectar clic y fijar el punto objetivo (target)
        if (Input.GetMouseButtonDown(0) && canWalk)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Cam.transform.position.z);
            Vector3 mouseWorldPos = Cam.ScreenToWorldPoint(mousePos);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, capaSuelo);

            if (hit.collider != null && hit.collider.CompareTag("piso"))
            {
                // Mantenemos la Z actual del personaje para que no cambie de plano
                target = new Vector3(hit.point.x, hit.point.y, transform.position.z);

                if (efectoClicPrefab != null)
                {
                    GameObject nuevoEfecto = Instantiate(efectoClicPrefab, new Vector3(target.x, target.y, 0f), Quaternion.identity);
                    Destroy(nuevoEfecto, tiempoDeVidaEfecto);
                }
            }
        }

        // 2. Mover el personaje usando transform.position
        float distancia = Vector3.Distance(transform.position, target);

        if (distancia > 0.05f)
        {
            // Mover progresivamente hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Dirección para animaciones y orientación (flip)
            float direccionX = target.x - transform.position.x;
            float direccionY = target.y - transform.position.y;

            if (animator != null)
            {
                animator.SetFloat("velocidadY", direccionY);
                animator.SetBool("estaCaminando", true);
            }

            // Girar el sprite según la dirección
            if (Mathf.Abs(direccionX) > 0.05f)
            {
                float escalaX = Mathf.Abs(transform.localScale.x);
                transform.localScale = new Vector3(direccionX > 0 ? escalaX : -escalaX, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            // Asegurar posición exacta al llegar
            transform.position = target;

            if (animator != null)
            {
                animator.SetBool("estaCaminando", false);
            }
        }

        // 3. Control de sonido
        if (animator != null)
        {
            caminando = animator.GetBool("estaCaminando");
            if (caminataSonido != null)
            {
                caminataSonido.SetActive(caminando);
            }
        }
    }

    public void HabilitarCamianata(bool habilitar)
    {
        canWalk = habilitar;
    }
}