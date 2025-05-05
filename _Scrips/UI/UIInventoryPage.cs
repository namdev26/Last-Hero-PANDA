using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private UIInventoryItemEquipment equipmentItemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private RectTransform usedItemsPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private ItemActionPanel actionPanel;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private UICharacterInfo characterInfo;

        private List<UIInventoryItem> listOfUIItem = new List<UIInventoryItem>();
        private Dictionary<EquipmentType, UIInventoryItemEquipment> usedItemSlots = new Dictionary<EquipmentType, UIInventoryItemEquipment>();
        [SerializeField] private Dictionary<EquipmentType, ItemSO> equippedItems = new Dictionary<EquipmentType, ItemSO>();

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        private int currentlyDraggedItemIndex = -1;
        private GameObject character;

        private void Awake()
        {
            equippedItems = new Dictionary<EquipmentType, ItemSO>();
            Hide();
            itemDescription.ResetDescription();
            mouseFollower.Toggle(false);

            UIInventoryItemEquipment[] equipmentSlots = usedItemsPanel.GetComponentsInChildren<UIInventoryItemEquipment>();
            foreach (var slot in equipmentSlots)
            {
                slot.ResetData();
            }
        }

        public void InitInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel, false);
                listOfUIItem.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnItemDropOn += HandleSwap;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }

            UIInventoryItemEquipment[] equipmentSlots = usedItemsPanel.GetComponentsInChildren<UIInventoryItemEquipment>();

            usedItemSlots[EquipmentType.Weapon] = equipmentSlots[0];
            usedItemSlots[EquipmentType.Weapon].Init(EquipmentType.Weapon);

            usedItemSlots[EquipmentType.Helmet] = equipmentSlots[1];
            usedItemSlots[EquipmentType.Helmet].Init(EquipmentType.Helmet);

            usedItemSlots[EquipmentType.Armor] = equipmentSlots[2];
            usedItemSlots[EquipmentType.Armor].Init(EquipmentType.Armor);

            usedItemSlots[EquipmentType.Boots] = equipmentSlots[3];
            usedItemSlots[EquipmentType.Boots].Init(EquipmentType.Boots);

            usedItemSlots[EquipmentType.Accessory] = equipmentSlots[4];
            usedItemSlots[EquipmentType.Accessory].Init(EquipmentType.Accessory);

            usedItemSlots[EquipmentType.Cape] = equipmentSlots[5];
            usedItemSlots[EquipmentType.Cape].Init(EquipmentType.Cape);

            foreach (EquipmentType type in usedItemSlots.Keys)
            {
                EquipmentType capturedType = type;
                usedItemSlots[capturedType].OnRightMouseBtnClick += (slotType) => HandleShowUsedItemActions(slotType);
                usedItemSlots[capturedType].OnItemClicked += HandleEquipmentItemSelection;
            }
        }

        private void HandleEquipmentItemSelection(UIInventoryItemEquipment equipmentItem)
        {
            ItemSO item = equipmentItem.GetItem();
            if (item == null)
            {
                itemDescription.ResetDescription();
                return;
            }

            string description = PrepareEquipmentDescription(item);
            itemDescription.SetDescription(item.ItemImage, item.name, description);
            DeselectAllItems();
        }

        private string PrepareEquipmentDescription(ItemSO item)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(item.Description);
            sb.AppendLine();
            foreach (var param in item.DefaultParametersList)
            {
                sb.Append($"{param.itemParameter.ParameterName}: {param.value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Show(GameObject characterRef)
        {
            character = characterRef;
            gameObject.SetActive(true);
            ResetSelection();

            if (character != null && characterInfo != null)
            {
                PlayerStats stats = character.GetComponent<PlayerStats>();
                if (stats != null)
                {
                    characterInfo.Initialize(stats);
                    characterInfo.Show();
                    UpdateCharacterStats(); // Cập nhật chỉ số khi mở inventory
                }
                else
                {
                    Debug.LogWarning("PlayerStats component not found on character.");
                }
            }
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
            if (characterInfo != null)
            {
                characterInfo.Hide();
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItem.Count > itemIndex)
            {
                if (itemImage == null || itemQuantity == 0)
                {
                    listOfUIItem[itemIndex].ResetData();
                }
                else
                {
                    listOfUIItem[itemIndex].SetData(itemImage, itemQuantity);
                }
            }
        }

        public void UpdateEquippedItem(EquipmentType slotType, Sprite itemImage, ItemSO item)
        {
            if (usedItemSlots.ContainsKey(slotType))
            {
                if (itemImage == null || item == null)
                {
                    usedItemSlots[slotType].ResetData();
                }
                else
                {
                    usedItemSlots[slotType].SetData(itemImage, item);
                }
            }
        }

        public Dictionary<EquipmentType, ItemSO> GetEquippedItems()
        {
            return equippedItems;
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1 || index >= inventoryData.GetCurrentInventoryState().Count)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleShowUsedItemActions(EquipmentType slotType)
        {
            if (!equippedItems.ContainsKey(slotType) || equippedItems[slotType] == null) return;

            actionPanel.Toggle(true);
            actionPanel.transform.position = usedItemSlots[slotType].transform.position;
            actionPanel.AddButon("Unequip", () => UnequipItem(slotType));
        }

        public void UseItem(int itemIndex, ItemSO item)
        {
            if (character == null || !(item is IItemAction itemAction)) return;

            EquipmentType slotType = item.equipmentType;

            if (equippedItems.ContainsKey(slotType) && equippedItems[slotType] != null)
            {
                UnequipItem(slotType);
                Debug.Log($"Đã gỡ trang bị {equippedItems[slotType]?.name} ra khỏi slot {slotType} tại vị trí {itemIndex}");
            }

            equippedItems[slotType] = item;
            usedItemSlots[slotType].SetData(item.ItemImage, item);

            UpdateData(itemIndex, null, 0);
            UpdateCharacterStats(); // Cập nhật chỉ số sau khi trang bị

            actionPanel.Toggle(false);
            Debug.Log($"Equip {item.name} vào slot {slotType}");
        }

        private void UnequipItem(EquipmentType slotType)
        {
            if (character == null || !equippedItems.ContainsKey(slotType) || equippedItems[slotType] == null) return;

            if (equippedItems[slotType] is IItemAction itemAction)
            {
                itemAction.PerformUnequipAction(character);
            }

            ItemSO item = equippedItems[slotType];

            inventoryData.AddItem(item, 1, item.DefaultParametersList);
            equippedItems[slotType] = null;
            usedItemSlots[slotType].ResetData();

            UpdateCharacterStats(); // Cập nhật chỉ số sau khi gỡ trang bị
            actionPanel.Toggle(false);
            Debug.Log($"Unequipped {item.name} from slot {slotType}");
        }

        private void UpdateCharacterStats()
        {
            PlayerStats stats = character?.GetComponent<PlayerStats>();
            if (stats == null)
            {
                Debug.LogWarning("PlayerStats component not found on character.");
                return;
            }

            stats.ResetAllBonuses(); // Reset tất cả bonus

            // Duyệt qua tất cả các vật phẩm đang trang bị và áp dụng bonus
            foreach (var kvp in equippedItems)
            {
                ItemSO item = kvp.Value;
                if (item == null) continue;

                Debug.Log($"Applying bonuses for item: {item.name} in slot {kvp.Key}");
                foreach (var param in item.DefaultParametersList)
                {
                    Debug.Log($"Parameter: {param.itemParameter.ParameterName}, Value: {param.value}");
                    stats.AddStatBonus(param.itemParameter.ParameterName, (int)param.value);
                }
            }

            Debug.Log($"Final stats - Damage: {stats.Damage}, Defence: {stats.Defence}, Health: {stats.Health}, Speed: {stats.Speed}");
        }

        private void HandleSwap(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1) return;
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(item);
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1) return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItem[itemIndex].transform.position;
        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItem)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButon(actionName, performAction);
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            if (itemIndex >= 0 && itemIndex < listOfUIItem.Count)
            {
                listOfUIItem[itemIndex].Select();
            }
        }

        public void ResetAllItems(bool resetEquippedItems = false)
        {
            foreach (var item in listOfUIItem)
            {
                if (item != null)
                {
                    item.ResetData();
                    item.Deselect();
                }
            }

            if (resetEquippedItems)
            {
                foreach (var slot in usedItemSlots.Values)
                {
                    if (slot != null)
                        slot.ResetData();
                }
                equippedItems.Clear();
                UpdateCharacterStats(); // Cập nhật chỉ số khi reset slot trang bị
            }
        }
    }
}