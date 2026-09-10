using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SonidosDeUI : MonoBehaviour
{
    public static SonidosDeUI instance;
    AudioSource source;

    public AudioSource Source { get { return source; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;

        if (source != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("El AudioSource en SonidosDeUI no est disponible");
        }
    }
    
}
