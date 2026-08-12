using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    //LECTOR DE PLAYERPREFS PARA BANDERAS DE JUEGO 

    public static GameStateManager Instance { get; private set; }

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
    }

    /// <summary>
    /// Método de utilidad para borrar la partida desde botones de UI o pruebas.
    /// </summary>
    public void BorrarTodoElProgreso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[GameStateManager] Progreso reiniciado completamente.");
    }
}