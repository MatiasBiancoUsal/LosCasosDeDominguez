using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    //LECTOR DE PLAYERPREFS PARA BANDERAS DE JUEGO 

    public static GameStateManager Instance { get; private set; }

    public System.Action<GameFlag> OnBanderaObtenida;

    private void Awake()
    {
        Debug.Log("🔵 GameStateManager Awake: " + gameObject.name +
                  " | Instance actual: " + (Instance != null ? Instance.gameObject.name : "NULL"));

        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("🔴 GameStateManager DUPLICADO. Destruyendo: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("🟢 GameStateManager PRINCIPAL: " + gameObject.name);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Consulta si una bandera ya fue registrada como cumplida (1).
    /// Si la bandera enviada es null, asume que no requiere condici�n y retorna true.
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
        Debug.Log($"[GameStateManager] Bandera guardada con �xito: {flag.name}");

        OnBanderaObtenida?.Invoke(flag);
    }

    /// <summary>
    /// M�todo de utilidad para borrar la partida desde botones de UI o pruebas.
    /// </summary>
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[GameStateManager] Progreso reiniciado completamente.");
    }

   
}