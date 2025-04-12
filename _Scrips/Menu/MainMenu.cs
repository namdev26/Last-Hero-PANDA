using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Bộ trộn âm thanh để điều chỉnh volume
    public Slider musicVolumeSlider; // Thanh kéo để điều chỉnh nhạc nền
    public Slider sfxVolumeSlider; // Thanh kéo để điều chỉnh hiệu ứng âm thanh

    private void Start()
    {
        LoadVolume(); // Tải âm lượng đã lưu
        MusicManager.Instance.PlayMusic("MainMenu"); // Phát nhạc menu chính
    }

    public void Play()
    {
        SceneManager.LoadScene("Game"); // Chuyển sang màn chơi
    }

    public void Quit()
    {
        Application.Quit(); // Thoát game
    }

    public void Options()
    {
        // Mở menu tuỳ chọn nếu bạn có UI cho nó
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume); // Cập nhật âm lượng nhạc nền
    }

    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume); // Cập nhật âm lượng hiệu ứng
    }

    public void SaveVolume()
    {
        // Lấy giá trị âm lượng hiện tại từ AudioMixer và lưu vào PlayerPrefs
        if (audioMixer.GetFloat("MusicVolume", out float musicVolume))
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        if (audioMixer.GetFloat("SFXVolume", out float sfxVolume))
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save(); // Lưu chắc chắn
    }

    public void LoadVolume()
    {
        // Lấy giá trị âm lượng đã lưu, nếu không có thì mặc định là 0
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);

        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        // Cập nhật âm lượng vào AudioMixer
        audioMixer.SetFloat("MusicVolume", musicVolume);
        audioMixer.SetFloat("SFXVolume", sfxVolume);
    }
}
