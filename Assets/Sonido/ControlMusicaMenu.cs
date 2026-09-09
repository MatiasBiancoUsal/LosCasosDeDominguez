using UnityEngine;

public class ControlMusicaMenu : MonoBehaviour
{
    public enum TipoDeMusica { menu, juego }
    public TipoDeMusica tipo;

    public enum MusicaDeMenu { si, no }
    public MusicaDeMenu usarMusica;

    void Start()
    {
        if(usarMusica == MusicaDeMenu.si)
        {
            if (tipo == TipoDeMusica.menu)
            {
                MusicMenu.instance.ActivarMusica = true;
            }

            if (tipo == TipoDeMusica.juego)
            {
                BgMusica.Instance.ActivarMusica = true;
            }
        }
        else
        {
            if (tipo == TipoDeMusica.menu)
            {
                MusicMenu.instance.ActivarMusica = false;
            }

            if (tipo == TipoDeMusica.juego)
            {
                BgMusica.Instance.ActivarMusica = false;
            }
        }
    }
}
