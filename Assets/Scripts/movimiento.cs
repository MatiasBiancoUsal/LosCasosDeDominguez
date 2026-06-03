using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Efecto de Clic")]
    [SerializeField] private GameObject efectoClicPrefab; 
    [SerializeField] private float tiempoDeVidaEfecto = 0.5f;

    private Vector2 target;
    private Camera Cam;

    void Start()
    {
        target = transform.position;
        Cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Cam.ScreenToWorldPoint(Input.mousePosition);
            target = new Vector2(worldPoint.x, worldPoint.y);

            if (efectoClicPrefab != null)
            {
                GameObject nuevoEfecto = Instantiate(efectoClicPrefab, new Vector3(target.x, target.y, 0f), Quaternion.identity);

                Destroy(nuevoEfecto, tiempoDeVidaEfecto);
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
