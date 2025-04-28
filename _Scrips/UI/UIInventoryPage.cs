
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

                uiItem.transform.SetParent(contentPanel);
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
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
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

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItem)
            {
                item.Deselect();
            }
            //actionPanel.Toggle(false);
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
    }
}