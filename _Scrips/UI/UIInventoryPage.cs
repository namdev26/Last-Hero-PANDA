using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private ItemActionPanel actionPanel;
        [SerializeField] private InventorySO inventoryData; // Thêm để truy cập kho
        [SerializeField] private AgentWeapon agentWeapon; // Thêm để truy cập AgentWeapon

        List<UIInventoryItem> listOfUIItem = new List<UIInventoryItem>();

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        private int currentlyDraggedItemIndex = -1;

        private void Awake()
        {
            Hide();
            itemDescription.ResetDescription();
            mouseFollower.Toggle(false);
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
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItem.Count > itemIndex)
            {
                listOfUIItem[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1 || index >= inventoryData.GetCurrentInventoryState().Count)
            {
                return;
            }

            OnItemActionRequested?.Invoke(index);

            // Kiểm tra xem vật phẩm có phải đang được trang bị
            var inventoryItem = inventoryData.GetItemAt(index);
            if (inventoryItem.item is EquipItemSO equipItem && agentWeapon != null && agentWeapon.GetCurrentWeapon() == equipItem)
            {
                actionPanel.AddButon("Unequip", () => PerformUnequip());
            }
        }

        private void HandleSwap(UIInventoryItem item)
        {
            int index = listOfUIItem.IndexOf(item);
            if (index == -1)
            {
                return;
            }
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
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
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
            foreach (UIInventoryItem platform in listOfUIItem)
            {
                platform.Deselect();
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
            listOfUIItem[itemIndex].Select();
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItem)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        // Phương thức để tháo trang bị
        private void PerformUnequip()
        {
            if (agentWeapon != null)
            {
                agentWeapon.Unequip();
                actionPanel.Toggle(false); // Ẩn panel hành động sau khi tháo
            }
        }
    }
}