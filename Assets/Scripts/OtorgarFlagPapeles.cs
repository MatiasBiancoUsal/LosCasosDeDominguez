
using UnityEngine;

public class OtorgarFlagPapeles : MonoBehaviour
{
    [Header("Flags necesarias (cualquiera de ellas)")]
    [SerializeField] private GameFlag flagNecesaria1;
    [SerializeField] private GameFlag flagNecesaria2;
    [SerializeField] private GameFlag flagNecesaria3;

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

        if (tieneFlag1 || tieneFlag2 || tieneFlag3)
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

        Debug.Log("Se obtuvo al menos una de las 3 flags. Flag otorgada: "
            + flagAotorgar.name);
    }
}
