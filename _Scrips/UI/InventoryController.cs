using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] UIInventoryPage inventoryPage;
        [SerializeField] private InventorySO inventorySOData;

        public List<InventoryItem> initItems = new List<InventoryItem>();

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventorySOData.Initialize();
            inventorySOData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initItems)
            {
                if (item.IsEmpty)
                    continue;
                inventorySOData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryPage.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryPage.UpdateData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryPage.InitInventoryUI(inventorySOData.Size);
            inventoryPage.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryPage.OnSwapItems += HandleSwap;
            inventoryPage.OnStartDragging += HandletDragging;
            inventoryPage.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySOData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject);
            }

            IDestroyableItem item = inventoryItem.item as IDestroyableItem;
            if (item != null)
            {
                inventorySOData.RemoveItem(itemIndex, 1);
            }
        }

        private void HandletDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySOData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryPage.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwap(int itemIndex_1, int itemIndex_2)
        {
            inventorySOData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventorySOData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryPage.ResetSelection();

                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryPage.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryPage.isActiveAndEnabled == false)
                {
                    inventoryPage.Show();
                    foreach (var item in inventorySOData.GetCurrentInventoryState())
                    {
                        inventoryPage.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryPage.Hide();
                }
            }
        }
    }
}