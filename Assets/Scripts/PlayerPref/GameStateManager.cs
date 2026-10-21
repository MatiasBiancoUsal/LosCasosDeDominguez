using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public System.Action<GameFlag> OnBanderaObtenida;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            SceneManager.sceneLoaded -= AlCargarEscena;
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    /// <summary> 
    /// Consulta si una bandera ya fue registrada como cumplida (1). 
    /// </summary>
    public bool TieneBandera(GameFlag flag)
    {
        if (flag == null) return true;

        // Lee directamente desde la memoria local de PlayerPrefs
        return PlayerPrefs.GetInt(flag.Id, 0) == 1;
    }

    /// <summary> 
    /// Guarda la bandera enviada en PlayerPrefs con valor 1 y fuerza la persistencia.
    /// </summary>
    public void GuardarBandera(GameFlag flag)
    {
        if (flag == null) return;

        PlayerPrefs.SetInt(flag.Id, 1);
        PlayerPrefs.Save(); // Guarda inmediatamente en disco para evitar pérdidas en transiciones

        Debug.Log($"[GameStateManager] BANDERA GUARDADA EXITOSAMENTE: {flag.name} (ID: {flag.Id})");

        OnBanderaObtenida?.Invoke(flag);
    }

    /// <summary> 
    /// Guarda el nombre personalizado de la habitación asociado a la bandera en PlayerPrefs.
    /// </summary>
    public void RegistrarHabitacionDesbloqueada(GameFlag flag, string nombreHabitacion)
    {
        if (flag == null || string.IsNullOrEmpty(nombreHabitacion))
            return;

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
        if (flag == null)
            return string.Empty;

        string claveGuardado = "NombreHabitacion_" + flag.Id;

        return PlayerPrefs.GetString(claveGuardado, "Sin nombre");
    }

    public void GuardarUltimaEscena()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("UltimaEscena", escenaActual);
        PlayerPrefs.Save();

        Debug.Log("[GameStateManager] Última habitación guardada: " + escenaActual);
    }

    public string ObtenerUltimaEscena()
    {
        return PlayerPrefs.GetString("UltimaEscena", "");
    }

    public bool HayPartidaGuardada()
    {
        return PlayerPrefs.HasKey("UltimaEscena");
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        string nombreEscena = escena.name;

        // Filtros de escenas que no cuentan como progreso
        if (nombreEscena == "Menu" ||
            nombreEscena == "Creditoscinematica" ||
            nombreEscena == "Cinematica 1" ||
            nombreEscena == "Cinematica 2")
        {
            return;
        }

        PlayerPrefs.SetString("UltimaEscena", nombreEscena);
        PlayerPrefs.Save();

        Debug.Log("[GameStateManager] Progreso guardado automáticamente en: " + nombreEscena);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("[GameStateManager] Progreso reiniciado completamente.");
    }
}