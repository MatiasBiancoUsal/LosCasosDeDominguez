using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private float speed;

<<<<<<< HEAD
    private Vector2 target;
    private Camera Cam;
=======
    [Header("Efecto de Clic")]
    [SerializeField] private GameObject efectoClicPrefab;
    [SerializeField] private float tiempoDeVidaEfecto = 0.5f;

    [Header("Configuracion de capas")]
    [SerializeField] private LayerMask capaSuelo;

    private Vector2 target;
    private Camera Cam;
    private Animator animator;
    private Rigidbody2D rb;
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c

    void Start()
    {
        target = transform.position;
        Cam = Camera.main;
<<<<<<< HEAD
=======
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
<<<<<<< HEAD
            Vector3 worldPoint = Cam.ScreenToWorldPoint(Input.mousePosition);
            target = new Vector2(worldPoint.x, worldPoint.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

}
=======
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
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c
