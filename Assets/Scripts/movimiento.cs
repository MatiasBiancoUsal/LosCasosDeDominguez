using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Efecto de Clic")]
    [SerializeField] private GameObject efectoClicPrefab;
    [SerializeField] private float tiempoDeVidaEfecto = 0.5f;

    [Header("Configuracion de capas")]
    [SerializeField] private LayerMask capaSuelo;

    private Vector2 target;
    private Camera Cam;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        target = transform.position;
        Cam = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Detectar el clic
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // Lanzamos el rayo para detectar si tocamos el suelo
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, capaSuelo);

            if (hit.collider != null)
            {
                // Si usas Tags, asegúrate de que el suelo se llame "piso" en minúsculas como acá
                if (hit.collider.CompareTag("piso"))
                {
                    target = hit.point;

                    if (efectoClicPrefab != null)
                    {
                        GameObject nuevoEfecto = Instantiate(efectoClicPrefab, new Vector3(target.x, target.y, 0f), Quaternion.identity);
                        Destroy(nuevoEfecto, tiempoDeVidaEfecto);
                    }
                }
            }
        }

        // 3. Controlar Animación y Giro (Flip)
        // Usamos una pequeña tolerancia (0.1f) para evitar que el personaje "tiemble" al llegar
        if (Vector2.Distance(transform.position, target) > 0.1f)
        {
            animator.SetBool("estaCaminando", true);

            if (target.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (target.x < transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            animator.SetBool("estaCaminando", false);
        }
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(rb.position, target) > 0.1f)
        {
            Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, target, speed *  Time.fixedDeltaTime);

            rb.MovePosition(nuevaPosicion);

        }
    }
}