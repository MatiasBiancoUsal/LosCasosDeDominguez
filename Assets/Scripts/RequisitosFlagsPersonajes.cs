

using UnityEngine;

public class RequisitosFlagsPersonajes : MonoBehaviour
{
    [Header("Flags necesarias")]
    [SerializeField] private GameFlag flagNecesaria1;
    [SerializeField] private GameFlag flagNecesaria2;
    [SerializeField] private GameFlag flagNecesaria3;
    [SerializeField] private GameFlag flagNecesaria4;
    [SerializeField] private GameFlag flagNecesaria5;
    [SerializeField] private GameFlag flagNecesaria6;
    [SerializeField] private GameFlag flagNecesaria7;
    [SerializeField] private GameFlag flagNecesaria8;
    [SerializeField] private GameFlag flagNecesaria9;

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
        bool tieneFlag5 = GameStateManager.Instance.TieneBandera(flagNecesaria5);
        bool tieneFlag6 = GameStateManager.Instance.TieneBandera(flagNecesaria6);
        bool tieneFlag7 = GameStateManager.Instance.TieneBandera(flagNecesaria7);
        bool tieneFlag8 = GameStateManager.Instance.TieneBandera(flagNecesaria8);
        bool tieneFlag9 = GameStateManager.Instance.TieneBandera(flagNecesaria9);

        if (tieneFlag1 && tieneFlag2 && tieneFlag3 &&
            tieneFlag4 && tieneFlag5 && tieneFlag6 &&
            tieneFlag7 && tieneFlag8 && tieneFlag9)
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

        Debug.Log("¡Se obtuvieron las 9 pistas! Flag otorgada: "
            + flagAotorgar.name);
    }
}