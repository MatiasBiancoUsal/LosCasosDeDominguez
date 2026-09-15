
using UnityEngine;

public class OtorgarFlag : MonoBehaviour
{
    [Header("Flag que se otorgará al entrar a esta escena")]
    [SerializeField] private GameFlag flagAlEntrar;

    private bool flagOtorgada = false;

    private void Start()
    {
        DarFlag();
    }

    private void DarFlag()
    {
        if (flagOtorgada)
            return;

        if (flagAlEntrar == null)
        {
            Debug.LogWarning("DarFlagAlEntrarEscena: No se asignó ninguna flag en el Inspector.");
            return;
        }

        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("DarFlagAlEntrarEscena: No se encontró GameStateManager.");
            return;
        }

        GameStateManager.Instance.GuardarBandera(flagAlEntrar);

        flagOtorgada = true;

        Debug.Log("Flag otorgada al entrar a la escena: " + flagAlEntrar.name);
    }
}
