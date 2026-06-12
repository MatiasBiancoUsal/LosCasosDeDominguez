using UnityEngine;

public class DesbloquearPistaTexto : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float velocidadFade = 3f;

    private bool desbloqueado = false;

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (desbloqueado && canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.MoveTowards(
                canvasGroup.alpha,
                1f,
                velocidadFade * Time.deltaTime
            );
        }
    }

    public void Desbloquear()
    {
        desbloqueado = true;
    }
}