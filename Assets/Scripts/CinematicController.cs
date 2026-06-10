using UnityEngine;
using UnityEngine.Video;
<<<<<<< HEAD
using System.Collections;
=======
using UnityEngine.SceneManagement;
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c

public class CinematicController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject cinematicPanel;

    [Header("Configuración de Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Objetos a pausar durante el video")]
    [SerializeField] private GameObject jugador;

<<<<<<< HEAD
=======
    [Header("Escena a cargar al finalizar")]
    [SerializeField] private string nombreEscena;

>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c
    void Start()
    {
        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(true);
        }
<<<<<<< HEAD
        //Para que el jugador no se pueda mover hasta que termine la cinematica
        if (jugador != null) jugador.SetActive(false);
=======

        // Para que el jugador no se pueda mover hasta que termine la cinemática
        if (jugador != null)
            jugador.SetActive(false);
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c

        if (videoPlayer != null)
        {
            videoPlayer.timeUpdateMode = VideoTimeUpdateMode.GameTime;
<<<<<<< HEAD

=======
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c
            videoPlayer.loopPointReached += OnCinematicEnd;
            videoPlayer.Play();
        }
        else
        {
            EndCinematic();
        }
    }

    void OnCinematicEnd(VideoPlayer vp)
    {
        EndCinematic();
    }

    public void EndCinematic()
    {
        Debug.Log("¡La cinemática terminó con éxito!");

<<<<<<< HEAD
        if (jugador != null) jugador.SetActive(true);

        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(false);
        }
=======
        if (jugador != null)
            jugador.SetActive(true);

        if (cinematicPanel != null)
            cinematicPanel.SetActive(false);

        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
>>>>>>> e664795eb9cbba5a7eebf0ddce98f61d5acdc02c
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnCinematicEnd;
        }
    }
}