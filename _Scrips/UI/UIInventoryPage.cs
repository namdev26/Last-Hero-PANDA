using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
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

        private List<UIInventoryItem> listOfUIItem = new();
        private Dictionary<EquipmentType, UIInventoryItemEquipment> usedItemSlots = new();
        private Dictionary<EquipmentType, ItemSO> equippedItems = new();

        private int currentlyDraggedItemIndex = -1;
        private GameObject character;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            Hide();
            itemDescription.ResetDescription();
            mouseFollower.Toggle(false);
            InitializeEquipmentSlots();
        }

        public void InitInventoryUI(int inventorySize)
        {
            CreateInventorySlots(inventorySize);
            InitializeEquipmentSlots();
        }

        public void Show(GameObject characterRef)
        {
            character = characterRef;
            gameObject.SetActive(true);
            ResetSelection();

            if (character != null && characterInfo != null)
            {
                if (character.TryGetComponent(out PlayerStats stats))
                {
                    characterInfo.Initialize(stats);
                    characterInfo.Show();
                    UpdateCharacterStats();
                }
                else
                {
                    Debug.LogWarning("Missing PlayerStats on character.");
                }
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            actionPanel.Toggle(false);
            ResetDraggedItem();
            characterInfo?.Hide();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int quantity)
        {
            if (itemIndex < listOfUIItem.Count)
            {
                if (itemImage == null || quantity == 0)
                    listOfUIItem[itemIndex].ResetData();
                else
                    listOfUIItem[itemIndex].SetData(itemImage, quantity);
            }
        }

        public void UpdateEquippedItem(EquipmentType type, Sprite image, ItemSO item)
        {
            if (!usedItemSlots.ContainsKey(type)) return;

            if (image == null || item == null)
                usedItemSlots[type].ResetData();
            else
                usedItemSlots[type].SetData(image, item);
        }

        public Dictionary<EquipmentType, ItemSO> GetEquippedItems() => equippedItems;

        public void UseItem(int itemIndex, ItemSO item)
        {
            if (character == null || item is not IItemAction) return;

            EquipmentType slotType = item.equipmentType;

            if (equippedItems.TryGetValue(slotType, out ItemSO equipped) && equipped != null)
                UnequipItem(slotType);

            equippedItems[slotType] = item;
            usedItemSlots[slotType].SetData(item.ItemImage, item);
            UpdateData(itemIndex, null, 0);
            UpdateCharacterStats();
            actionPanel.Toggle(false);

            Debug.Log($"Equipped {item.name} to {slotType}");
        }

        private void UnequipItem(EquipmentType slotType)
        {
            if (character == null || !equippedItems.TryGetValue(slotType, out ItemSO item) || item == null) return;

            if (item is IItemAction action)
                action.PerformUnequipAction(character);

            inventoryData.AddItem(item, 1, item.DefaultParametersList);
            equippedItems[slotType] = null;
            usedItemSlots[slotType].ResetData();
            UpdateCharacterStats();

            actionPanel.Toggle(false);
            Debug.Log($"Unequipped {item.name} from {slotType}");
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public void ShowItemAction(int index)
        {
            if (index >= 0 && index < listOfUIItem.Count)
            {
                actionPanel.Toggle(true);
                actionPanel.transform.position = listOfUIItem[index].transform.position;
            }
        }

        public void AddAction(string name, Action action) => actionPanel.AddButon(name, action);

        public void UpdateDescription(int index, Sprite image, string name, string desc)
        {
            itemDescription.SetDescription(image, name, desc);
            DeselectAllItems();
            if (index >= 0 && index < listOfUIItem.Count)
                listOfUIItem[index].Select();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void ResetAllItems(bool resetEquipped = false)
        {
            foreach (var item in listOfUIItem)
            {
                item?.ResetData();
                item?.Deselect();
            }

            if (resetEquipped)
            {
                foreach (var slot in usedItemSlots.Values)
                    slot?.ResetData();

                equippedItems.Clear();
                UpdateCharacterStats();
            }
        }

        // ========== PRIVATE METHODS ==========

        private void InitializeEquipmentSlots()
        {
            UIInventoryItemEquipment[] slots = usedItemsPanel.GetComponentsInChildren<UIInventoryItemEquipment>();
            EquipmentType[] types = (EquipmentType[])Enum.GetValues(typeof(EquipmentType));

            for (int i = 0; i < types.Length && i < slots.Length; i++)
            {
                EquipmentType type = types[i];
                usedItemSlots[type] = slots[i];
                usedItemSlots[type].Init(type);

                usedItemSlots[type].OnRightMouseBtnClick += (_) => HandleShowUsedItemActions(type);
                usedItemSlots[type].OnItemClicked += HandleEquipmentItemSelection;
            }
        }

        private void CreateInventorySlots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemPrefab, contentPanel);
                listOfUIItem.Add(item);

                item.OnItemClicked += HandleItemSelection;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnItemDropOn += HandleSwap;
                item.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index >= 0)
                OnDescriptionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1) return;

            currentlyDraggedItemIndex = index;
            OnStartDragging?.Invoke(index);
            HandleItemSelection(item);
        }

        private void HandleEndDrag(UIInventoryItem _) => ResetDraggedItem();

        private void HandleSwap(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1) return;

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(item);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void DeselectAllItems()
        {
            foreach (var item in listOfUIItem)
                item.Deselect();

            actionPanel.Toggle(false);
        }

        private void HandleEquipmentItemSelection(UIInventoryItemEquipment equipmentItem)
        {
            var item = equipmentItem.GetItem();
            if (item == null)
            {
                itemDescription.ResetDescription();
                return;
            }

            itemDescription.SetDescription(item.ItemImage, item.name, PrepareEquipmentDescription(item));
            DeselectAllItems();
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index >= 0 && index < inventoryData.GetCurrentInventoryState().Count)
                OnItemActionRequested?.Invoke(index);
        }

        private void HandleShowUsedItemActions(EquipmentType type)
        {
            if (equippedItems.TryGetValue(type, out ItemSO item) && item != null)
            {
                actionPanel.Toggle(true);
                actionPanel.transform.position = usedItemSlots[type].transform.position;
                actionPanel.AddButon("Unequip", () => UnequipItem(type));
            }
        }

        private string PrepareEquipmentDescription(ItemSO item)
        {
            var sb = new System.Text.StringBuilder(item.Description).AppendLine();
            foreach (var param in item.DefaultParametersList)
                sb.AppendLine($"{param.itemParameter.ParameterName}: {param.value}");

            return sb.ToString();
        }

        private void UpdateCharacterStats()
        {
            if (!character.TryGetComponent(out PlayerStats stats))
            {
                Debug.LogWarning("PlayerStats component not found.");
                return;
            }

            stats.ResetAllBonuses();

            foreach (var pair in equippedItems)
            {
                if (pair.Value == null) continue;

                foreach (var param in pair.Value.DefaultParametersList)
                {
                    stats.AddStatBonus(param.itemParameter.ParameterName, (int)param.value);
                }
            }

            CheckAndApplySetBonus(stats);
        }

        private void CheckAndApplySetBonus(PlayerStats stats)
        {
            Dictionary<int, int> setCounts = new Dictionary<int, int>();

            foreach (var item in equippedItems.Values)
            {
                if (item == null) continue;

                int setId = item.SetId;
                if (setId > 0) // Kiểm tra nếu item có SetId hợp lệ
                {
                    if (!setCounts.ContainsKey(setId))
                        setCounts[setId] = 0;

                    setCounts[setId]++;
                }
            }

            // Duyệt qua các set đã được xác định và áp dụng bonus nếu đủ số lượng
            foreach (var kvp in setCounts)
            {
                int setId = kvp.Key;
                int setCount = kvp.Value;

                // Áp dụng bonus cho các set khi đủ số lượng
                switch (setId)
                {
                    case 1: // Set Tăng 15% Sát thương
                        if (setCount >= 5)
                        {
                            stats.AddStatBonus("SucManh", 1000); // Cộng bonus sức mạnh cho set 1
                            Debug.Log($"Set Bonus Activated for Set ID: {setId} (+1000 Damage)");
                        }
                        break;

                    case 2: // Tăng 10% né tránh sát thương
                        if (setCount >= 3)
                        {
                            stats.AddStatBonus("KienCuong", 500); // Cộng bonus phòng thủ cho set 2
                            Debug.Log($"Set Bonus Activated for Set ID: {setId} (+500 Defence)");
                        }
                        break;

                    case 3: // Hồi 2% HP mỗi 5 giây
                        if (setCount >= 4)
                        {
                            stats.AddStatBonus("BenBi", 800); // Cộng bonus máu cho set 3
                            Debug.Log($"Set Bonus Activated for Set ID: {setId} (+800 Health)");
                        }
                        break;

                    case 4: // Tăng 10% tất cả chỉ số
                        if (setCount >= 2)
                        {
                            stats.AddStatBonus("KheoLeo", 300); // Cộng bonus tốc độ cho set 4
                            Debug.Log($"Set Bonus Activated for Set ID: {setId} (+300 Speed)");
                        }
                        break;

                    default:
                        Debug.LogWarning($"Unknown Set ID: {setId}");
                        break;
                }
            }
        }

    }
}
