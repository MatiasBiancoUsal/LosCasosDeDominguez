using UnityEngine;

public class DesbloquearPistaTexto : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float velocidadFade = 3f;

    private bool desbloqueado = false;

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