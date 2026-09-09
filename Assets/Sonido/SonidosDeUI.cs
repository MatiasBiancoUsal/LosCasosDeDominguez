using UnityEngine;

public class SonidosDeUI : MonoBehaviour
{
    public static SonidosDeUI instance;
    AudioSource source;
    public AudioClip _botones;

    public AudioSource Source { get { return source; } }

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

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            if(_botones != null)
            {
                source.PlayOneShot(_botones);
            }
        }
    }
}
