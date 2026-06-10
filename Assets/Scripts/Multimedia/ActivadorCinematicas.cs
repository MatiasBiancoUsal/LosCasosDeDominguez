using UnityEngine;
using UnityEngine.Video;

public class ActivadorCinematicas : MonoBehaviour
{
    public Modo _modo;
    public VideoPlayer _videoPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_modo == Modo.auto)
        {
            _videoPlayer.Play();
        }
    }

    public void Reproducir()
    {
        _videoPlayer.Play();
    }
}
