using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorLlamada : MonoBehaviour
{
    public void AceptarLlamada()
    {
        print($"[Analytics] Atender llamada -> Acepto: {true}");

        // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (ISA)
        // EnviarDato.Singleton.EventoAtenderLlamada(true);

        SceneManager.LoadScene(3);
    }

    public void RechazarLlamada()
    {
        print($"[Analytics] Atender llamada -> Acepto: {false}");

        // SACAR DE COMENTARIO ASI SE ENVIA EL DATO (ISA)
        // EnviarDato.Singleton.EventoAtenderLlamada(false);

        SceneManager.LoadScene("Menu");
    }
}
