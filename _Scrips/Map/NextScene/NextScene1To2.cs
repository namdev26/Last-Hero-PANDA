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
            // Kiểm tra tính hợp lệ của targetSceneName và loadingSceneName
            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogError("targetSceneName is empty or null in NextScene1To2!", this);
                return;
            }

            if (string.IsNullOrEmpty(loadingSceneName))
            {
                Debug.LogError("loadingSceneName is empty or null in NextScene1To2!", this);
                return;
            }

            // Kiểm tra xem cảnh có trong Build Settings không
            bool isValidTargetScene = false;
            bool isValidLoadingScene = false;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneNameInBuild == targetSceneName)
                    isValidTargetScene = true;
                if (sceneNameInBuild == loadingSceneName)
                    isValidLoadingScene = true;
            }

            if (!isValidTargetScene)
            {
                Debug.LogError($"Target scene '{targetSceneName}' not found in Build Settings!", this);
                return;
            }

            if (!isValidLoadingScene)
            {
                Debug.LogError($"Loading scene '{loadingSceneName}' not found in Build Settings!", this);
                return;
            }

            // Lưu tên scene đích vào PlayerPrefs
            PlayerPrefs.SetString("NextSceneName", targetSceneName);
            PlayerPrefs.Save();
            Debug.Log($"Saved target scene '{targetSceneName}' to PlayerPrefs for loading.");

            // Load scene loading
            SceneManager.LoadScene(loadingSceneName);
            Debug.Log($"Triggering load of '{loadingSceneName}' scene.");
        }
        else
        {
            Debug.LogWarning($"Collider {collision.gameObject.name} does not have tag 'Player'.", this);
        }
    }

    private void OnValidate()
    {
        // Kiểm tra trong Editor để đảm bảo Collider là trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger)
        {
            Debug.LogWarning($"Collider on {gameObject.name} is not set to 'Is Trigger'. This may prevent OnTriggerEnter2D from working.", this);
        }
    }
}