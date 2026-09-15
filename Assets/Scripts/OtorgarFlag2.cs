
using UnityEngine;

public class OtorgarFlag2 : MonoBehaviour
{
    [Header("Flags necesarias")]
    [SerializeField] private GameFlag flagNecesaria1;
    [SerializeField] private GameFlag flagNecesaria2;

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

        if (tieneFlag1 && tieneFlag2)
        {
            OtorgarFlagFinal();
        }
    }

    private void OtorgarFlagFinal()
    {
        if (flagAotorgar == null)
        {
            Debug.LogWarning("RequisitosDosFlags: No se asignó la flag que se debe otorgar.");
            return;
        }

        GameStateManager.Instance.GuardarBandera(flagAotorgar);

        flagFinalYaOtorgada = true;

        Debug.Log(
            "Se obtuvieron las 2 flags necesarias. Flag otorgada: "
            + flagAotorgar.name
        );
    }
}
