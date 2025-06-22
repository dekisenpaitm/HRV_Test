using UnityEngine;

public class Soundmanager : MonoBehaviour
{
    public static Soundmanager Instance;
    [SerializeField] private AudioClip BackgroundA;
    [SerializeField] private AudioClip BackgroundB;
    [SerializeField] private AudioClip Error;

    [SerializeField] private AudioSource background;
    [SerializeField] private AudioSource sfx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void PlayBackgroundA()
    {
        background.PlayOneShot(BackgroundA);
    }

    public void PlayBackgroundB()
    {
        background.PlayOneShot(BackgroundB);
    }

    public void PlayError()
    {
        sfx.PlayOneShot(Error);
    }

    public void StopBackgroundAudio()
    {
        background.Stop();
    }
}
