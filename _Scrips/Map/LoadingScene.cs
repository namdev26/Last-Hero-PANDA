using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private static string nextSceneName = ""; // Giá trị mặc định nếu không có scene nào được chỉ định
    private const string NEXT_SCENE_KEY = "NextSceneName"; // Key dùng để lưu tên scene tiếp theo trong PlayerPrefs

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading Scene"); // Tên scene loading của bạn
    }

    private void Start()
    {
        // Nếu không có tên scene được truyền vào, thử lấy từ PlayerPrefs
        string targetScene = string.IsNullOrEmpty(nextSceneName) ? 
            PlayerPrefs.GetString(NEXT_SCENE_KEY, "") : 
            nextSceneName;

        // Nếu vẫn không có tên scene, mặc định về scene đầu tiên (hoặc scene lỗi)
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("No target scene specified!");
            targetScene = SceneManager.GetActiveScene().name; // Mặc định load lại scene hiện tại
        }

        StartCoroutine(LoadYourAsyncScene(targetScene));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // Xóa dữ liệu đã lưu để tránh xung đột cho lần sau
        if (PlayerPrefs.HasKey(NEXT_SCENE_KEY))
        {
            PlayerPrefs.DeleteKey(NEXT_SCENE_KEY);
            PlayerPrefs.Save();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Hiển thị loading progress
        while (asyncLoad.progress < 0.9f)
        {
            float progressValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (progressValue * 100) + "%");
            yield return null;
        }

        // Đợi thêm 1 giây để đảm bảo mọi thứ đã sẵn sàng
        yield return new WaitForSeconds(1f);

        // Cho phép chuyển cảnh
        asyncLoad.allowSceneActivation = true;
    }
}