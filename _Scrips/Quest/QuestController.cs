﻿using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;
using TMPro;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

public class QuestController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questUI;
    public Button acceptButton;
    public Button completeButton;
    public Button closeButton;
    public Button nextButton; // Nút "Tiếp"
    public TextMeshProUGUI questDescriptionText;

    [Header("Quest Settings")]
    public InventorySO inventoryData;
    public ItemSO requiredItem;
    public int requiredAmount = 5;
    public ItemSO rewardItem;
    public int rewardAmount = 1;

    [Header("Quest Descriptions")]
    [TextArea] public string[] notStartedDescription = new string[] { };
    [TextArea] public string[] inProgressDescription = new string[] { };
    [TextArea] public string[] completedDescription = new string[] { };

    [Header("NPC Interaction")]
    public Transform npcTransform;
    public Transform playerTransform;
    public float interactionDistance = 3f;

    private QuestState currentState = QuestState.NotStarted;
    private bool isPanelOpen = false;
    private int currentDialogueIndex = 0;

    public delegate void QuestStateChanged();
    public event QuestStateChanged OnQuestStateChanged;

    private void Start()
    {
        if (!ValidateReferences()) return;

        questUI.SetActive(false);
        acceptButton.onClick.AddListener(AcceptQuest);
        completeButton.onClick.AddListener(CompleteQuest);
        closeButton.onClick.AddListener(ClosePanel);
        nextButton.onClick.AddListener(ShowNextDialogue);

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPanelOpen)
            {
                if (Vector3.Distance(playerTransform.position, npcTransform.position) <= interactionDistance)
                {
                    ToggleQuestUI();
                }
                else
                {
                }
            }
            else
            {
                ClosePanel();
            }
        }

        if (currentState == QuestState.InProgress && questUI.activeSelf)
        {
            UpdateUI();
        }
    }

    public void ToggleQuestUI()
    {
        if (!ValidateReferences()) return;

        questUI.SetActive(!questUI.activeSelf);
        isPanelOpen = questUI.activeSelf;
        if (isPanelOpen)
        {
            currentDialogueIndex = 0;
            UpdateUI();
        }
    }

    private void AcceptQuest()
    {
        if (currentState != QuestState.NotStarted) return;

        currentState = QuestState.InProgress;
        currentDialogueIndex = 0;
        UpdateUI();
        OnQuestStateChanged?.Invoke();
    }

    private void CompleteQuest()
    {
        if (currentState != QuestState.InProgress || !HasEnoughItems()) return;

        currentState = QuestState.Completed;
        inventoryData.RemoveItem(requiredItem, requiredAmount);
        inventoryData.AddItem(rewardItem, rewardAmount);

        currentDialogueIndex = 0;
        UpdateUI();
        OnQuestStateChanged?.Invoke();
    }

    private bool HasEnoughItems()
    {
        if (inventoryData == null || requiredItem == null) return false;
        return inventoryData.GetItemCount(requiredItem) >= requiredAmount;
    }

    public QuestState GetCurrentState()
    {
        return currentState;
    }

    private void UpdateUI()
    {
        if (!ValidateReferences()) return;

        string[] currentDescription = currentState switch
        {
            QuestState.NotStarted => notStartedDescription,
            QuestState.InProgress => inProgressDescription,
            QuestState.Completed => completedDescription,
            _ => notStartedDescription
        };

        currentDialogueIndex = Mathf.Clamp(currentDialogueIndex, 0, currentDescription.Length - 1);

        if (currentDialogueIndex < currentDescription.Length)
        {
            string textToDisplay = currentDescription[currentDialogueIndex];
            switch (currentState)
            {
                case QuestState.NotStarted:
                    questDescriptionText.text = string.Format(textToDisplay, requiredAmount, requiredItem.Name);
                    break;
                case QuestState.InProgress:
                    questDescriptionText.text = string.Format(textToDisplay, requiredItem.Name, inventoryData.GetItemCount(requiredItem), requiredAmount);
                    break;
                case QuestState.Completed:
                    questDescriptionText.text = textToDisplay;
                    break;
            }
        }

        nextButton.interactable = currentDialogueIndex < currentDescription.Length - 1;
        acceptButton.interactable = currentState == QuestState.NotStarted;
        completeButton.interactable = currentState == QuestState.InProgress && HasEnoughItems();
    }

    private void ShowNextDialogue()
    {
        string[] currentDescription = currentState switch
        {
            QuestState.NotStarted => notStartedDescription,
            QuestState.InProgress => inProgressDescription,
            QuestState.Completed => completedDescription,
            _ => notStartedDescription
        };

        if (currentDialogueIndex < currentDescription.Length - 1)
        {
            currentDialogueIndex++;
            UpdateUI();
            Debug.Log($"Chuyển sang đoạn {currentDialogueIndex}: {currentDescription[currentDialogueIndex]}"); // Debug
        }
    }

    private void ClosePanel()
    {
        questUI.SetActive(false);
        isPanelOpen = false;
        currentDialogueIndex = 0;
    }

    private bool ValidateReferences()
    {
        if (questUI == null || acceptButton == null || completeButton == null || closeButton == null || nextButton == null || questDescriptionText == null)
        {
            return false;
        }
        if (inventoryData == null || requiredItem == null || rewardItem == null)
        {
            return false;
        }
        if (npcTransform == null || playerTransform == null)
        {
            return false;
        }
        return true;
    }

    private void OnDestroy()
    {
        if (acceptButton != null) acceptButton.onClick.RemoveListener(AcceptQuest);
        if (completeButton != null) completeButton.onClick.RemoveListener(CompleteQuest);
        if (closeButton != null) closeButton.onClick.RemoveListener(ClosePanel);
        if (nextButton != null) nextButton.onClick.RemoveListener(ShowNextDialogue);
    }
}