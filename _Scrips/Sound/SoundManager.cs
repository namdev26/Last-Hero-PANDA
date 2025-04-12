using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private SoundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clip);
        }
    }

    public void PlaySound3D(string soundName, Vector3 position)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), position);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    public void PlayMusic2D(string musicName)
    {
        var clip = musicLibrary.GetMusicFromName(musicName);
        if (clip != null)
            musicSource.PlayOneShot(clip);
        else
            Debug.LogWarning("Music clip not found: " + musicName);
    }
}
