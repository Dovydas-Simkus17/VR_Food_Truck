using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager sharedInstance;
    public AudioSource musicSource;
    public AudioClip baseSong;
    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void changeSong(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
}