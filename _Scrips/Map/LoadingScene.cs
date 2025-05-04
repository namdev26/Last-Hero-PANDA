using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private void Start()
    {
        string nextSceneName = PlayerPrefs.GetString("NextSceneName", "Dark Area");
        StartCoroutine(LoadYourAsyncScene(nextSceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            float progressValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (progressValue * 100) + "%");
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        asyncLoad.allowSceneActivation = true;
    }
}