using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    // Lớp chính điều phối UI
    public class UIInventoryPage : MonoBehaviour
    {
        [Header("Prefabs & UI References")]
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private UIInventoryItemEquipment equipmentItemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private RectTransform usedItemsPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private ItemActionPanel actionPanel;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private UICharacterInfo characterInfo;
        [SerializeField] private GameObject Set1Effect;
        [SerializeField] private GameObject Set2Effect;
        [SerializeField] private GameObject Set3Effect;
        [SerializeField] private GameObject Set4Effect;
        [SerializeField] private PlayerHealth playerHealth;

        private InventorySlotManager slotManager;
        private EquipmentManager equipmentManager;
        private CharacterStatsUpdater statsUpdater;
        private GameObject character;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            slotManager = new InventorySlotManager(itemPrefab, contentPanel, mouseFollower);
            equipmentManager = new EquipmentManager(equipmentItemPrefab, usedItemsPanel, inventoryData);
            statsUpdater = new CharacterStatsUpdater(playerHealth, new GameObject[] { Set1Effect, Set2Effect, Set3Effect, Set4Effect });

            Hide();
            itemDescription.ResetDescription();
            mouseFollower.Toggle(false);
            equipmentManager.InitializeEquipmentSlots();
        }

        public void InitInventoryUI(int inventorySize)
        {
            slotManager.CreateInventorySlots(inventorySize, HandleItemSelection, HandleBeginDrag, HandleEndDrag, HandleSwap, HandleShowItemActions);
            equipmentManager.InitializeEquipmentSlots();
        }

        public void Show(GameObject characterRef)
        {
            character = characterRef;
            gameObject.SetActive(true);
            slotManager.ResetSelection(itemDescription);

            if (character != null && characterInfo != null)
            {
                if (character.TryGetComponent(out PlayerStats stats))
                {
                    characterInfo.Initialize(stats);
                    characterInfo.Show();
                    statsUpdater.UpdateCharacterStats(character, equipmentManager.GetEquippedItems());
                }
                else
                {
                    Debug.LogWarning("Thiếu thành phần PlayerStats trên nhân vật.");
                }
            }
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            actionPanel.Toggle(false);
            slotManager.ResetDraggedItem();
            characterInfo?.Hide();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int quantity)
        {
            slotManager.UpdateData(itemIndex, itemImage, quantity);
        }

        public void UpdateEquippedItem(EquipmentType type, Sprite image, ItemSO item)
        {
            equipmentManager.UpdateEquippedItem(type, image, item);
        }

        public Dictionary<EquipmentType, ItemSO> GetEquippedItems() => equipmentManager.GetEquippedItems();

        public void UseItem(int itemIndex, ItemSO item)
        {
            equipmentManager.UseItem(item, character, () => slotManager.UpdateData(itemIndex, null, 0), () => statsUpdater.UpdateCharacterStats(character, equipmentManager.GetEquippedItems()));
            actionPanel.Toggle(false);
        }

        public void ShowItemAction(int index)
        {
            slotManager.ShowItemAction(index, actionPanel);
        }

        public void AddAction(string name, Action action) => actionPanel.AddButon(name, action);

        public void UpdateDescription(int index, Sprite image, string name, string desc)
        {
            itemDescription.SetDescription(image, name, desc);
            slotManager.DeselectAllItems();
            slotManager.SelectItem(index);
        }

        public void ResetSelection() => slotManager.ResetSelection(itemDescription);

        public void ResetAllItems(bool resetEquipped = false)
        {
            slotManager.ResetAllItems();
            if (resetEquipped)
            {
                equipmentManager.ResetAllEquippedItems();
                statsUpdater.UpdateCharacterStats(character, equipmentManager.GetEquippedItems());
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = slotManager.GetItemIndex(item);
            if (index >= 0)
                OnDescriptionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = slotManager.GetItemIndex(item);
            if (index >= 0)
            {
                OnStartDragging?.Invoke(index);
                HandleItemSelection(item);
            }
        }

        private void HandleEndDrag(UIInventoryItem _) => slotManager.ResetDraggedItem();

        private void HandleSwap(UIInventoryItem item)
        {
            int index = slotManager.GetItemIndex(item);
            if (index >= 0)
            {
                OnSwapItems?.Invoke(slotManager.CurrentlyDraggedItemIndex, index);
                HandleItemSelection(item);
            }
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = slotManager.GetItemIndex(item);
            if (index >= 0 && index < inventoryData.GetCurrentInventoryState().Count)
                OnItemActionRequested?.Invoke(index);
        }
    }

}