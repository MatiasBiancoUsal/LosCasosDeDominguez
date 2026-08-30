using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // LECTOR DE PLAYERPREFS PARA BANDERAS DE JUEGO 

    public static GameStateManager Instance { get; private set; }

    public System.Action<GameFlag> OnBanderaObtenida;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Consulta si una bandera ya fue registrada como cumplida (1).
    /// Si la bandera enviada es null, asume que no requiere condición y retorna true.
    /// </summary>
    public bool TieneBandera(GameFlag flag)
    {
        if (flag == null) return true;
        return PlayerPrefs.GetInt(flag.Id, 0) == 1;
    }

    /// <summary>
    /// Guarda la bandera enviada en PlayerPrefs con valor 1.
    /// </summary>
    public void GuardarBandera(GameFlag flag)
    {
        if (flag == null) return;

        PlayerPrefs.SetInt(flag.Id, 1);
        PlayerPrefs.Save();
        Debug.Log($"[GameStateManager] Bandera guardada con éxito: {flag.name}");

        OnBanderaObtenida?.Invoke(flag);
    }

    /// <summary>
    /// Guarda el nombre personalizado de la habitación asociado a la bandera en PlayerPrefs.
    /// </summary>
    public void RegistrarHabitacionDesbloqueada(GameFlag flag, string nombreHabitacion)
    {
        if (flag == null || string.IsNullOrEmpty(nombreHabitacion)) return;

        // Guarda en PlayerPrefs usando una clave única ("NombreHabitacion_IDDeLaBandera")
        string claveGuardado = "NombreHabitacion_" + flag.Id;
        PlayerPrefs.SetString(claveGuardado, nombreHabitacion);
        PlayerPrefs.Save();

        Debug.Log($"[GameStateManager] Habitación '{nombreHabitacion}' registrada para la bandera: {flag.name}");
    }

    /// <summary>
    /// Devuelve el nombre que le asignó el jugador a la habitación de esa bandera.
    /// </summary>
    public string ObtenerNombreHabitacion(GameFlag flag)
    {
        if (flag == null) return string.Empty;

        string claveGuardado = "NombreHabitacion_" + flag.Id;
        return PlayerPrefs.GetString(claveGuardado, "Sin nombre");
    }

    /// <summary>
    /// Método de utilidad para borrar la partida desde botones de UI o pruebas.
    /// </summary>
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[GameStateManager] Progreso reiniciado completamente.");
    }
}