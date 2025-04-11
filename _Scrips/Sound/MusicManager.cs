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

    public void PlayMusic(string musicName, float duration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(MusicLibrary.GetMusicFromName(musicName), duration));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float duration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            musicSource.volume = Mathf.Lerp(0, 1, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            musicSource.volume = Mathf.Lerp(1, 0, percent);
            yield return null;
        }
    }
}
