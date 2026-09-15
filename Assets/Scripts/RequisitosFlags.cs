
using UnityEngine;

public class RequisitosFlags : MonoBehaviour
{
    [Header("Flags necesarias")]
    [SerializeField] private GameFlag flagNecesaria1;
    [SerializeField] private GameFlag flagNecesaria2;
    [SerializeField] private GameFlag flagNecesaria3;
    [SerializeField] private GameFlag flagNecesaria4;

    [Header("Flag a otorgar")]
    [SerializeField] private GameFlag flagAotorgar;

    private bool flagFinalYaOtorgada = false;

    private void Update()
    {
        ComprobarFlags();
    }

    private void ComprobarFlags()
    {
        if (flagFinalYaOtorgada)
            return;

        if (GameStateManager.Instance == null)
            return;

        bool tieneFlag1 = GameStateManager.Instance.TieneBandera(flagNecesaria1);
        bool tieneFlag2 = GameStateManager.Instance.TieneBandera(flagNecesaria2);
        bool tieneFlag3 = GameStateManager.Instance.TieneBandera(flagNecesaria3);
        bool tieneFlag4 = GameStateManager.Instance.TieneBandera(flagNecesaria4);

        if (tieneFlag1 && tieneFlag2 && tieneFlag3 && tieneFlag4)
        {
            OtorgarFlagFinal();
        }
    }

    private void OtorgarFlagFinal()
    {
        if (flagAotorgar == null)
        {
            Debug.LogWarning("No se asignó la flag que se debe otorgar.");
            return;
        }

        GameStateManager.Instance.GuardarBandera(flagAotorgar);

        flagFinalYaOtorgada = true;

        Debug.Log("¡Se obtuvieron las 4 pistas! Flag otorgada: " + flagAotorgar.name);
    }
}
