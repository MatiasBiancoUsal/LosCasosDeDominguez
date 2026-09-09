using UnityEngine;
using UnityEngine.Audio;

public class MusicMenu : MonoBehaviour
{
    public static MusicMenu instance;
    public AudioMixerGroup mixer;
    bool activarMusica = true;
    float musicVolume;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        if (activarMusica)
        {
            musicVolume = Mathf.MoveTowards(musicVolume, 0, Time.deltaTime * 25);
        }
        else
        {
            musicVolume = Mathf.MoveTowards(musicVolume, -80, Time.deltaTime * 25);
        }

        mixer.audioMixer.SetFloat("MenuVolume", musicVolume);
    }

    // Acá controlamos la múscia del menú para que suene suave

    public bool ActivarMusica { get { return activarMusica; } set { activarMusica = value; } }
}
