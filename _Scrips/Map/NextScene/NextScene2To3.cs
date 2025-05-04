using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene2To3 : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Boss"; // Scene đích
    [SerializeField] private string loadingSceneName = "Loading Scene"; // Scene loading

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerModel"))
        {
            // Lưu tên scene đích vào PlayerPrefs để scene loading có thể đọc được
            PlayerPrefs.SetString("NextSceneName", targetSceneName);
            PlayerPrefs.Save();

            // Load scene loading
            SceneManager.LoadScene(loadingSceneName);
        }
    }
}