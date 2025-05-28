using UnityEngine;

public class NPCTalk : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    private DialogueController dialogueController;

    void Start()
    {
        FindDialogueController();

        // Kiểm tra DialogueText
        if (dialogueText == null)
        {
            Debug.LogError($"{name}: DialogueText is not assigned! Please assign it in Inspector.");
        }
    }

    private void FindDialogueController()
    {
        // Cách 1: Tìm DialogueController trong scene (bao gồm DontDestroyOnLoad)
        dialogueController = FindObjectOfType<DialogueController>(true); // true = includeInactive

        // Cách 2: Nếu không tìm thấy, tìm trong DontDestroyOnLoad scene
        if (dialogueController == null)
        {
            // Tìm trong DontDestroyOnLoad objects
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObj in rootObjects)
            {
                if (rootObj.scene.name == "DontDestroyOnLoad")
                {
                    dialogueController = rootObj.GetComponentInChildren<DialogueController>(true);
                    if (dialogueController != null)
                        break;
                }
            }
        }

        // Cách 3: Tìm trong tất cả scenes đã load
        if (dialogueController == null)
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                GameObject[] sceneRootObjects = scene.GetRootGameObjects();

                foreach (GameObject rootObj in sceneRootObjects)
                {
                    dialogueController = rootObj.GetComponentInChildren<DialogueController>(true);
                    if (dialogueController != null)
                        break;
                }

                if (dialogueController != null)
                    break;
            }
        }

        // Cách 4: Tìm qua tất cả GameObject, kể cả DontDestroyOnLoad
        if (dialogueController == null)
        {
            DialogueController[] allControllers = Resources.FindObjectsOfTypeAll<DialogueController>();
            foreach (DialogueController controller in allControllers)
            {
                // Kiểm tra xem có phải là object trong scene không (không phải prefab)
                if (controller.gameObject.scene.IsValid())
                {
                    dialogueController = controller;
                    break;
                }
            }
        }

        if (dialogueController == null)
        {
            Debug.LogError($"{name}: DialogueController not found in any scene or DontDestroyOnLoad! Make sure DialogueController exists.");
        }
        else
        {
            Debug.Log($"{name}: Successfully found DialogueController on {dialogueController.gameObject.name} in scene: {dialogueController.gameObject.scene.name}");
        }
    }

    public override void Interact()
    {
        if (dialogueText != null)
        {
            Talk(dialogueText);
        }
        else
        {
            Debug.LogWarning($"{name}: Cannot interact - DialogueText is null!");
        }
    }

    public void Talk(DialogueText dialogueText)
    {
        if (dialogueController == null)
        {
            Debug.LogError($"{name}: DialogueController is null! Cannot display dialogue.");
            return;
        }

        if (dialogueText == null)
        {
            Debug.LogError($"{name}: DialogueText is null! Cannot display dialogue.");
            return;
        }

        dialogueController.DisplayNextParagraph(dialogueText);
    }
}