using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance { get; private set; }

    [Header("Atajos de Teclado")]
    [SerializeField] private KeyCode teclaOtorgarBanderas = KeyCode.F1;
    [SerializeField] private KeyCode teclaSaltarEscena = KeyCode.F2;

    [Header("Configuración de Cheats")]
    [Tooltip("Arrastra aquí todas las banderas que quieras otorgar con el truco.")]
    [SerializeField] private GameFlag[] banderasADesbloquear;

    [Tooltip("Escena a la que saltarás al presionar la tecla de salto.")]
    [SerializeField] private string escenaDestino;

    private void Awake()
    {
        // Mantiene una sola instancia viva entre cambios de escena
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Truco 1: Desbloquear todas las banderas requeridas
        if (Input.GetKeyDown(teclaOtorgarBanderas))
        {
            CompletarTodasLasBanderas();
        }

        // Truco 2: Cargar la escena final o siguiente directamente
        if (Input.GetKeyDown(teclaSaltarEscena))
        {
            SaltarDeEscena();
        }
    }

    public void CompletarTodasLasBanderas()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("[CHEAT] GameStateManager no encontrado en la escena.");
            return;
        }

        foreach (var bandera in banderasADesbloquear)
        {
            if (bandera != null)
            {
                GameStateManager.Instance.GuardarBandera(bandera);
            }
        }

        Debug.Log("<color=green>[CHEAT ACTIVADO]</color> Se otorgaron todas las banderas. ¡Paneles e interacciones desbloqueados!");
    }

    public void SaltarDeEscena()
    {
        if (!string.IsNullOrEmpty(escenaDestino))
        {
            Debug.Log($"<color=yellow>[CHEAT ACTIVADO]</color> Saltando a la escena: {escenaDestino}");
            SceneManager.LoadScene(escenaDestino);
        }
        else
        {
            Debug.LogWarning("[CHEAT] No asignaste un nombre de escena destino en el Inspector.");
        }
    }
}
