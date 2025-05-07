using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestStatusUI : MonoBehaviour
{
    public GameObject statusPanel;
    public TextMeshProUGUI statusText;
    public QuestController questController;

    private void Start()
    {
        statusPanel.SetActive(false);
    }

    public void ToggleStatusPanel()
    {
        statusPanel.SetActive(!statusPanel.activeSelf);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (questController == null) return;

        var state = questController.GetCurrentState();
        var currentItem = questController.requiredItem;
        var count = questController.inventoryData.GetItemCount(currentItem);

        switch (state)
        {
            case QuestState.NotStarted:
                statusText.text = "Chưa nhận nhiệm vụ.";
                break;
            case QuestState.InProgress:
                statusText.text = $"Đang làm nhiệm vụ:\nThu thập {count}/{questController.requiredAmount} {currentItem.Name}";
                break;
            case QuestState.Completed:
                statusText.text = "✅ Đã hoàn thành nhiệm vụ. Hãy đến gặp NPC.";
                break;
        }
    }
}
