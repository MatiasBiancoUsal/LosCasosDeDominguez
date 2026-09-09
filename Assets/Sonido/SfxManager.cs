using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
    AudioSource source;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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

    public void PlaySfx(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
