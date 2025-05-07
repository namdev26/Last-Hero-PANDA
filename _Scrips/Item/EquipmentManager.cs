using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager
{
    private readonly UIInventoryItemEquipment equipmentItemPrefab;
    private readonly RectTransform usedItemsPanel;
    private readonly InventorySO inventoryData;
    private readonly Dictionary<EquipmentType, UIInventoryItemEquipment> usedItemSlots = new();
    private readonly Dictionary<EquipmentType, ItemSO> equippedItems = new();

    public EquipmentManager(UIInventoryItemEquipment equipmentItemPrefab, RectTransform usedItemsPanel, InventorySO inventoryData)
    {
        this.equipmentItemPrefab = equipmentItemPrefab;
        this.usedItemsPanel = usedItemsPanel;
        this.inventoryData = inventoryData;
    }

    public void InitializeEquipmentSlots()
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

    public void UpdateEquippedItem(EquipmentType type, Sprite image, ItemSO item)
    {
        if (!usedItemSlots.ContainsKey(type)) return;

        if (image == null || item == null)
            usedItemSlots[type].ResetData();
        else
            usedItemSlots[type].SetData(image, item);
    }

    public Dictionary<EquipmentType, ItemSO> GetEquippedItems() => equippedItems;

    public void UseItem(ItemSO item, GameObject character, Action onUpdateInventory, Action onUpdateStats)
    {
        if (character == null || item is not IItemAction) return;

        EquipmentType slotType = item.equipmentType;

        if (equippedItems.TryGetValue(slotType, out ItemSO equipped) && equipped != null)
            UnequipItem(slotType);

        equippedItems[slotType] = item;
        usedItemSlots[slotType].SetData(item.ItemImage, item);
        onUpdateInventory?.Invoke();
        onUpdateStats?.Invoke();

        Debug.Log($"Đã trang bị {item.name} vào {slotType}");
    }

    public void UnequipItem(EquipmentType slotType)
    {
        if (!equippedItems.TryGetValue(slotType, out ItemSO item) || item == null) return;

        if (item is IItemAction action)
            action.PerformUnequipAction(null); // Giả sử không cần character

        inventoryData.AddItem(item, 1, item.DefaultParametersList);
        equippedItems[slotType] = null;
        usedItemSlots[slotType].ResetData();

        Debug.Log($"Đã tháo {item.name} khỏi {slotType}");
    }

    public void ResetAllEquippedItems()
    {
        foreach (var slot in usedItemSlots.Values)
            slot?.ResetData();
        equippedItems.Clear();
    }

    private void HandleShowUsedItemActions(EquipmentType type)
    {
        // Logic hiển thị hành động cho trang bị đã sử dụng
    }

    private void HandleEquipmentItemSelection(UIInventoryItemEquipment equipmentItem)
    {
        // Logic xử lý khi chọn trang bị
    }
}

// Cập nhật chỉ số nhân vật
public class CharacterStatsUpdater
{
    private readonly PlayerHealth playerHealth;
    private readonly GameObject[] setEffects;

    public CharacterStatsUpdater(PlayerHealth playerHealth, GameObject[] setEffects)
    {
        this.playerHealth = playerHealth;
        this.setEffects = setEffects;
    }

    public void UpdateCharacterStats(GameObject character, Dictionary<EquipmentType, ItemSO> equippedItems)
    {
        if (!character.TryGetComponent(out PlayerStats playerStats))
        {
            Debug.LogWarning("Không tìm thấy thành phần PlayerStats.");
            return;
        }

        playerStats.ResetAllBonuses();

        foreach (var pair in equippedItems)
        {
            if (pair.Value == null) continue;

            foreach (var param in pair.Value.DefaultParametersList)
            {
                playerStats.AddStatBonus(param.itemParameter.ParameterName, (int)param.value);
            }
        }

        CheckAndApplySetBonus(playerStats, equippedItems);
    }

    private void CheckAndApplySetBonus(PlayerStats playerStats, Dictionary<EquipmentType, ItemSO> equippedItems)
    {
        Dictionary<int, int> setCounts = new Dictionary<int, int>();

        // Đếm số lượng món của từng bộ
        foreach (var item in equippedItems.Values)
        {
            if (item == null) continue;

            int setId = item.SetId;
            if (setId > 0)
            {
                if (!setCounts.ContainsKey(setId))
                    setCounts[setId] = 0;
                setCounts[setId]++;
            }
        }

        playerHealth.SetRegenSetActive(false);
        foreach (var effect in setEffects)
            effect.SetActive(false);

        foreach (var kvp in setCounts)
        {
            int setId = kvp.Key;
            int setCount = kvp.Value;

            switch (setId)
            {
                case 1:
                    if (setCount >= 5)
                    {
                        playerStats.Set1();
                        setEffects[0].SetActive(true);
                    }
                    break;
                case 2:
                    if (setCount >= 5)
                    {
                        playerStats.Set2();
                        setEffects[1].SetActive(true);
                    }
                    break;
                case 3:
                    if (setCount >= 5)
                    {
                        playerHealth.SetRegenSetActive(true);
                        setEffects[2].SetActive(true);
                    }
                    break;
                case 4:
                    if (setCount >= 5)
                    {
                        playerStats.Set4();
                        setEffects[3].SetActive(true);
                    }
                    break;
                default:
                    Debug.LogWarning($"ID bộ không xác định: {setId}");
                    break;
            }
        }
    }
}
