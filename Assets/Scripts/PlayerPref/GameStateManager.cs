using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (flag == null || string.IsNullOrEmpty(nombreHabitacion))
            return;

        string claveGuardado = "NombreHabitacion_" + flag.Id;

        PlayerPrefs.SetString(claveGuardado, nombreHabitacion);
        PlayerPrefs.Save();

        Debug.Log(
            $"[GameStateManager] Habitación '{nombreHabitacion}' registrada para la bandera: {flag.name}"
        );
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

    // Guarda automáticamente la habitación actual.
    public void GuardarUltimaEscena()
    {
        string escenaActual = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("UltimaEscena", escenaActual);
        PlayerPrefs.Save();

        Debug.Log(
            "[GameStateManager] Última habitación guardada: "
            + escenaActual
        );
    }

    // Devuelve el nombre de la última habitación.
    public string ObtenerUltimaEscena()
    {
        return PlayerPrefs.GetString("UltimaEscena", "");
    }

    // Comprueba si existe una partida guardada.
    public bool HayPartidaGuardada()
    {
        return PlayerPrefs.HasKey("UltimaEscena");
    }


    // Cada vez que Unity carga una escena,guardamos automáticamente su nombre.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        string nombreEscena = escena.name;

        //El menú no se guarda como progreso
        if (nombreEscena == "Menu")
            return;

        //Los créditos y cinemáticas no son puntos de guardado para continuar
        if (nombreEscena == "Creditoscinematica")
            return;

        if (nombreEscena == "Cinematica 1")
            return;

        if (nombreEscena == "Cinematica 2")
            return;

        //Guardamos la habitación.
        PlayerPrefs.SetString("UltimaEscena", nombreEscena);
        PlayerPrefs.Save();

        Debug.Log(
            "[GameStateManager] Progreso guardado automáticamente en: "
            + nombreEscena
        );
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