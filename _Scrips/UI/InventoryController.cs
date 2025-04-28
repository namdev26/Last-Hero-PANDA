using Inventory.Model;
using Inventory.UI;
using Iventory.Model;
using System;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] UIInventoryPage inventoryPage;
        [SerializeField] private InventorySO inventorySOData;


        private void Start()
        {
            PrepareUI();
            //inventorySOData.Initialize();
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
            throw new NotImplementedException();
        }

        private void HandletDragging(int itemIndex)
        {
            throw new NotImplementedException();
        }

        private void HandleSwap(int itemIndex_1, int itemIndex_2)
        {
            throw new NotImplementedException();
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