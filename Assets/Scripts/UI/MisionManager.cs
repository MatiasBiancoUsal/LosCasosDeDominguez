using UnityEngine;

public class MisionManager : MonoBehaviour
{
    public Mision[] misiones;

    private void Start()
    {
        Debug.Log("=== MISION MANAGER INICIADO ===");
        Debug.Log("Escena: " + gameObject.scene.name);

        if (GameStateManager.Instance == null)
        {
            Debug.LogError("ERROR: GameStateManager.Instance es NULL.");
            return;
        }

        Debug.Log("GameStateManager encontrado.");

        GameStateManager.Instance.OnBanderaObtenida += AlObtenerBandera;

        Debug.Log("MisionManager conectado a OnBanderaObtenida.");

        // Revisar todas las flags que ya estaban conseguidas
        ActualizarTodasLasMisiones();
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
        if (bandera == null)
            return;

        Debug.Log("=== MISION MANAGER RECIBIÓ FLAG ===");
        Debug.Log("Flag obtenida: " + bandera.name);

        ActualizarTodasLasMisiones();
    }

    private void ActualizarTodasLasMisiones()
    {
        if (GameStateManager.Instance == null)
            return;

        if (misiones == null || misiones.Length == 0)
        {
            Debug.LogWarning("MisionManager no tiene misiones asignadas.");
            return;
        }

        foreach (Mision mision in misiones)
        {
            if (mision == null)
                continue;

            bool activada = false;
            bool completada = false;

            if (mision.flagActivacion != null)
            {
                activada = GameStateManager.Instance.TieneBandera(
                    mision.flagActivacion
                );
            }

            if (mision.flagCompletada != null)
            {
                completada = GameStateManager.Instance.TieneBandera(
                    mision.flagCompletada
                );
            }

            bool debeEstarActiva = activada && !completada;

            Debug.Log(
                "Misión: " + mision.gameObject.name +
                " | Activada: " + activada +
                " | Completada: " + completada +
                " | Mostrar: " + debeEstarActiva
            );

            mision.gameObject.SetActive(debeEstarActiva);
        }
    }
}