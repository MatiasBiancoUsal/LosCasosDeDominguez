using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject cinematicPanel;

    [Header("Configuración de Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Objetos a pausar durante el video")]
    [SerializeField] private GameObject jugador;

    [Header("Escena a cargar al finalizar")]
    [SerializeField] private string nombreEscena;

    void Start()
    {
        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(true);
        }

        // Para que el jugador no se pueda mover hasta que termine la cinemática
        if (jugador != null)
            jugador.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.timeUpdateMode = VideoTimeUpdateMode.GameTime;
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

        if (jugador != null)
            jugador.SetActive(true);

        if (cinematicPanel != null)
            cinematicPanel.SetActive(false);

        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnCinematicEnd;
        }
    }
}