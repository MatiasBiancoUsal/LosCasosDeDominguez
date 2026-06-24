using UnityEngine;

public class BibliotecaIluminada : MonoBehaviour
{
    [SerializeField] private SpriteRenderer libros;
    [SerializeField] private Collider2D colliderLibros;

    [SerializeField] private int sortingOscuro = -10;
    [SerializeField] private int sortingIluminado = 10;

    private void Start()
    {
        if (EstadoJuego.tieneLampara)
        {
            libros.sortingOrder = sortingIluminado;

            if (colliderLibros != null)
                colliderLibros.enabled = true;
        }
        else
        {
            libros.sortingOrder = sortingOscuro;

            if (colliderLibros != null)
                colliderLibros.enabled = false;
        }
    }
}