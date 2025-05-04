using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene1To2 : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Dark Area"; // Scene đích
    [SerializeField] private string loadingSceneName = "Loading Scene"; // Scene loading

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Lưu tên scene đích vào PlayerPrefs để scene loading có thể đọc được
            PlayerPrefs.SetString("NextSceneName", targetSceneName);
            PlayerPrefs.Save();

            // Load scene loading
            SceneManager.LoadScene(loadingSceneName);
        }
    }
}