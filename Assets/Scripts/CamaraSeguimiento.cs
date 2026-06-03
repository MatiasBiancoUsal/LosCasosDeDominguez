using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    [SerializeField] private Transform objetivo; 
    [SerializeField] private float suavizado = 5f; 
    [SerializeField] private Vector3 desfase = new Vector3(0, 0, -10); 

    void LateUpdate()
    {
        if (objetivo != null)
        {
            Vector3 posicionDeseada = objetivo.position + desfase;

            Vector3 posicionSuave = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

            transform.position = posicionSuave;
        }
    }
}
