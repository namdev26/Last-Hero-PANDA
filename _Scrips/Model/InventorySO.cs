using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (!item.IsStackable)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                }
                InformAboutChange();
                return quantity;
            }

            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState ?? item.DefaultParametersList)
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }

            return 0;
        }

        private bool IsInventoryFull()
        {
            return inventoryItems.All(item => !item.IsEmpty);
        }

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                if (inventoryItems[i].item.ID != item.ID) continue;

                int maxToAdd = item.MaxStackSize - inventoryItems[i].quantity;

                if (quantity > maxToAdd)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(item.MaxStackSize);
                    quantity -= maxToAdd;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int toAdd = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= toAdd;
                AddItemToFirstFreeSlot(item, toAdd);
            }

            return quantity;
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if (itemIndex >= inventoryItems.Count || inventoryItems[itemIndex].IsEmpty) return;

            int remaining = inventoryItems[itemIndex].quantity - amount;

            if (remaining <= 0)
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            else
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remaining);

            InformAboutChange();
        }

        public void RemoveItem(ItemSO item, int amount)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty || inventoryItems[i].item.ID != item.ID) continue;

                if (inventoryItems[i].quantity > amount)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity - amount);
                    break;
                }
                else
                {
                    amount -= inventoryItems[i].quantity;
                    inventoryItems[i] = InventoryItem.GetEmptyItem();
                }

                if (amount <= 0) break;
            }

            InformAboutChange();
        }

        public int GetItemCount(ItemSO item)
        {
            int count = 0;
            foreach (var invItem in inventoryItems)
            {
                if (!invItem.IsEmpty && invItem.item.ID == item.ID)
                {
                    count += invItem.quantity;
                }
            }
            return count;
        }

        public bool HasFreeSpace()
        {
            return inventoryItems.Any(item => item.IsEmpty);
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity, item.itemState);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> state = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].IsEmpty)
                {
                    state[i] = inventoryItems[i];
                }
            }
            return state;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int index1, int index2)
        {
            InventoryItem temp = inventoryItems[index1];
            inventoryItems[index1] = inventoryItems[index2];
            inventoryItems[index2] = temp;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
        }
    }
}
