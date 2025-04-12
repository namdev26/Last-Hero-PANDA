using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrary MusicLibrary;
    [SerializeField] private AudioSource musicSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string musicName)
    {
        musicSource.clip = MusicLibrary.GetMusicFromName(musicName);
        musicSource.Play();
    }

}
