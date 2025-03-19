using UnityEngine;

public class NextScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplicationVariables.LoadingSceneName = "Dark Area";
            SceneController.instance.LoadScene("Loading Scene");
        }
    }
}
