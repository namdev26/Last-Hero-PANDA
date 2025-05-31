using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private static string nextSceneName = "";
    private const string NEXT_SCENE_KEY = "NextSceneName";

    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Tên scene không hợp lệ hoặc rỗng!");
            return;
        }

        // Kiểm tra xem scene có trong Build Settings không
        bool isValidScene = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameInBuild == sceneName)
            {
                isValidScene = true;
                break;
            }
        }

        if (!isValidScene)
        {
            Debug.LogError($"Scene '{sceneName}' không tồn tại trong Build Settings!");
            return;
        }

        nextSceneName = sceneName;
        Debug.Log($"Static LoadScene called with scene '{sceneName}'. Loading 'Loading Scene'.");
        SceneManager.LoadScene("Loading Scene");
    }

    private void Start()
    {
        // Ưu tiên lấy từ PlayerPrefs trước, sau đó mới đến nextSceneName
        string targetScene = PlayerPrefs.GetString(NEXT_SCENE_KEY, "");
        if (string.IsNullOrEmpty(targetScene))
        {
            targetScene = nextSceneName;
        }

        // Nếu không có tên scene đích, log lỗi và dừng
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("No target scene specified in PlayerPrefs or nextSceneName! Cannot proceed.");
            return;
        }

        // Kiểm tra xem targetScene có trong Build Settings không
        bool isValidScene = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameInBuild == targetScene)
            {
                isValidScene = true;
                break;
            }
        }

        if (!isValidScene)
        {
            Debug.LogError($"Target scene '{targetScene}' not found in Build Settings! Cannot proceed.");
            return;
        }

        Debug.Log($"Target scene determined: '{targetScene}'.");

        // Tìm PlayerController và vô hiệu hóa điều khiển
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.CanControl = false;
            Debug.Log("PlayerController found and control disabled.");
        }
        else
        {
            Debug.LogWarning("PlayerController not found in the scene!");
        }

        // Đặt lại nextSceneName để tránh xung đột cho lần sau
        nextSceneName = "";
        Debug.Log("nextSceneName reset to empty string.");

        StartCoroutine(LoadYourAsyncScene(targetScene));
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // Xóa PlayerPrefs để tránh xung đột
        if (PlayerPrefs.HasKey(NEXT_SCENE_KEY))
        {
            PlayerPrefs.DeleteKey(NEXT_SCENE_KEY);
            PlayerPrefs.Save();
            Debug.Log($"Cleared PlayerPrefs key '{NEXT_SCENE_KEY}'.");
        }

        // Tải cảnh bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene '{sceneName}'. Ensure it is added to Build Settings.");
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        // Hiển thị tiến trình tải
        while (asyncLoad.progress < 0.9f)
        {
            float progressValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log($"Loading progress for '{sceneName}': {(progressValue * 100):F1}%");
            yield return null;
        }

        // Đợi thêm 1 giây để đảm bảo sẵn sàng
        yield return new WaitForSeconds(1f);

        // Cho phép chuyển cảnh
        asyncLoad.allowSceneActivation = true;
        Debug.Log($"Scene '{sceneName}' activated.");

        // Kích hoạt lại điều khiển người chơi
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.CanControl = true;
            Debug.Log("PlayerController control enabled after scene load.");
        }
        else
        {
            Debug.LogWarning("PlayerController not found after scene load!");
        }
    }
}