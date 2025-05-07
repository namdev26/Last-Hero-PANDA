using UnityEngine;

public class NPCQuest : NPC
{
    [Header("Quest References")]
    [SerializeField] private QuestController questController;

    private void Start()
    {
        if (questController == null)
        {
            Debug.LogError("NPCQuest: QuestController không được gán!");
            return;
        }
    }

    public override void Interact()
    {
        if (questController == null) return;

        questController.ToggleQuestUI();
    }
}