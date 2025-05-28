using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        LoadingScene.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        if (audioMixer.GetFloat("MusicVolume", out float musicVolume))
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }
        if (audioMixer.GetFloat("SFXVolume", out float sfxVolume))
        {
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("MusicVolume", musicVolume);
            musicVolumeSlider?.SetValueWithoutNotify(musicVolume);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            audioMixer.SetFloat("SFXVolume", sfxVolume);
            sfxVolumeSlider?.SetValueWithoutNotify(sfxVolume);
        }
    }
}