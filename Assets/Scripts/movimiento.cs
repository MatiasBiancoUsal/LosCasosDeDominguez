using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    [SerializeField] private float speed;

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
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

}
