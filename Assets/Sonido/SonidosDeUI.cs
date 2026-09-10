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
        source.PlayOneShot(clip);
    }
}
