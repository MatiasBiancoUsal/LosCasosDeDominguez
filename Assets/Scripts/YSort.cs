using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    public float sortingOffset = 0f;
    private SpriteRenderer sr;
    private SpriteRenderer[] childRenderers;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Guardamos los SpriteRenderer de los objetos hijos
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        float y = transform.position.y + sortingOffset;
        int baseOrder = Mathf.RoundToInt(-y * 100f);

        // Asignamos el orden a la Mesa
        sr.sortingOrder = baseOrder;

        // Iteramos los objetos hijos para darles orden
        foreach (SpriteRenderer childSr in childRenderers)
        {
            if (childSr != sr)
            {
                // Si el objeto hijo está dentro de un Canvas (como el hover UI), lo ignoramos
                if (childSr.GetComponentInParent<Canvas>() != null)
                {
                    continue;
                }

                // A los sprites normales sobre la mesa (Libreta, Teléfono) les damos baseOrder + 1
                childSr.sortingOrder = baseOrder + 1;
            }
        }
    }
}