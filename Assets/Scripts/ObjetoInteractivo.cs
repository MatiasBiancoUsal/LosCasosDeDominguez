using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup _canvasG;

    [Header("Canvas Investigación")]
    public GameObject canvasInvestigacion;

    [Header("Efecto Fade")]
    public Vector2 _fadeRango = new Vector2(0, 1);
    public float _velocidad = 5;

    [Header("Editor")]
    public Color _visualizador = Color.green;

    bool isFaded;
    bool puedeInvestigar;

    private void Update()
    {
        // Fade del hover
        if (isFaded)
        {
            _canvasG.alpha = Mathf.MoveTowards(_canvasG.alpha, _fadeRango.y, Time.deltaTime * _velocidad);
        }
        else
        {
            _canvasG.alpha = Mathf.MoveTowards(_canvasG.alpha, _fadeRango.x, Time.deltaTime * _velocidad);
        }

        // Abrir investigación con Q
        if (puedeInvestigar && Input.GetKeyDown(KeyCode.Q))
        {
            canvasInvestigacion.SetActive(true);
        }
    }

    public void Mostrar()
    {
        isFaded = true;
        puedeInvestigar = true;
    }

    public void Ocultar()
    {
        isFaded = false;
        puedeInvestigar = false;
    }

    private void OnDrawGizmos()
    {
        PolygonCollider2D pc2d = GetComponent<PolygonCollider2D>();

        if (pc2d == null) return;

        Vector2[] points = pc2d.points;

        if (points.Length < 2) return;

        Gizmos.color = _visualizador;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 currentPoint = points[i];
            Vector2 nextPoint = points[(i + 1) % points.Length];
            Gizmos.DrawLine(transform.TransformPoint(currentPoint), transform.TransformPoint(nextPoint));
        }
    }
}
