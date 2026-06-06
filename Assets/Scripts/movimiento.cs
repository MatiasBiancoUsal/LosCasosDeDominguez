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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, capaSuelo);

            if (hit.collider != null)
            {
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

        if (Vector2.Distance(rb.position, target) > 0.15f)
        {
            animator.SetBool("estaCaminando", true);
        }
        else
        {
            animator.SetBool("estaCaminando", false);
        }
    }

    private void FixedUpdate()
    {
        float distancia = Vector2.Distance(rb.position, target);

        if (distancia > 0.15f)
        {
            Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

            float direccionHaciaTargetX = target.x - rb.position.x;

            if (Mathf.Abs(direccionHaciaTargetX) > 0.05f)
            {
                if (direccionHaciaTargetX > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }

            rb.MovePosition(nuevaPosicion);
        }
        else
        {
            rb.position = target;
        }
    }
}