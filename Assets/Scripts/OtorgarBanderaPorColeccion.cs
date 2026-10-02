using UnityEngine;

public class OtorgarBanderaPorColeccion : MonoBehaviour
{
    [Header("Flags requeridas para la colección")]
    public GameFlag flag1;
    public GameFlag flag2;
    public GameFlag flag3;

    [Header("Flag que se otorgará al conseguir las 3")]
    public GameFlag banderaAOtorgar;

    private void Start()
    {
        if (GameStateManager.Instance == null)
            return;

        // Comprobamos al iniciar la escena por si ya las tenía de antes
        VerificarColeccion();

        // Escuchamos cada vez que el jugador obtiene una bandera nueva
        GameStateManager.Instance.OnBanderaObtenida += AlObtenerBandera;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnBanderaObtenida -= AlObtenerBandera;
        }
    }

    private void AlObtenerBandera(GameFlag bandera)
    {
        // Si la bandera obtenida es una de las 3, verificamos la colección
        if (bandera == flag1 || bandera == flag2 || bandera == flag3)
        {
            VerificarColeccion();
        }
    }

    private void VerificarColeccion()
    {
        if (GameStateManager.Instance == null || banderaAOtorgar == null)
            return;

        // Comprobamos si las 3 banderas individuales están guardadas en PlayerPrefs
        bool tiene1 = flag1 != null && GameStateManager.Instance.TieneBandera(flag1);
        bool tiene2 = flag2 != null && GameStateManager.Instance.TieneBandera(flag2);
        bool tiene3 = flag3 != null && GameStateManager.Instance.TieneBandera(flag3);

        if (tiene1 && tiene2 && tiene3)
        {
            // Si ya tiene las 3 y todavía no tiene la bandera otorgada, la guarda
            if (!GameStateManager.Instance.TieneBandera(banderaAOtorgar))
            {
                GameStateManager.Instance.GuardarBandera(banderaAOtorgar);
                Debug.Log($"[OtorgarBanderaPorColeccion] ¡Colección completa! Bandera '{banderaAOtorgar.name}' otorgada exitosamente.");
            }
        }
    }
}