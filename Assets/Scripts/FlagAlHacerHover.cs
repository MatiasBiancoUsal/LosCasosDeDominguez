using UnityEngine;

[RequireComponent(typeof(DetectorHover))]
public class FlagAlHacerHover : MonoBehaviour
{
    [Header("Flag a otorgar al hacer hover")]
    [SerializeField] private GameFlag flag;

    private DetectorHover detectorHover;
    private bool yaComprobo;

    private void Awake()
    {
        detectorHover = GetComponent<DetectorHover>();
    }

    private void Update()
    {
        if (yaComprobo)
            return;

        if (flag == null)
            return;

        if (detectorHover.MouseEstaEncima)
        {
            OtorgarFlag();
        }
    }

    private void OtorgarFlag()
    {
        yaComprobo = true;

        // Si la flag ya estaba obtenida, no hacemos nada.
        if (GameStateManager.Instance.TieneBandera(flag))
        {
            Debug.Log($"[FlagAlHacerHover] La bandera '{flag.name}' ya estaba obtenida.");
            return;
        }

        GameStateManager.Instance.GuardarBandera(flag);

        Debug.Log($"[FlagAlHacerHover] Flag obtenida por hover: {flag.name}");
    }
}