using UnityEngine;
using UnityEngine.Events;

public class SonidoRemoto : MonoBehaviour
{
    public enum ModoDeEventos { llamada, start, enable, disable, enableAndDisable }
    public ModoDeEventos modo;
    public UnityEvent evento;
    public UnityEvent evento2;

    private void Start()
    {
        if (modo == ModoDeEventos.start)
        {
            evento.Invoke();
        }
    }

    private void OnEnable()
    {
        if (modo == ModoDeEventos.enable)
        {
            evento.Invoke();
        }

        if (modo == ModoDeEventos.enableAndDisable)
        {
            evento.Invoke();
        }
    }

    private void OnDisable()
    {
        if (modo == ModoDeEventos.disable)
        {
            evento.Invoke();
        }

        if (modo == ModoDeEventos.enableAndDisable)
        {
            evento2.Invoke();
        }
    }

    public void SonidoUIPlay(AudioClip clip)
    {
        SonidosDeUI.instance.Play(clip);
    }
}
