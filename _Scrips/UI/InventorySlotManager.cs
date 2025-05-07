using Inventory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager
{
    private readonly UIInventoryItem itemPrefab;
    private readonly RectTransform contentPanel;
    private readonly MouseFollower mouseFollower;
    private readonly List<UIInventoryItem> listOfUIItem = new();
    private int currentlyDraggedItemIndex = -1;

    public int CurrentlyDraggedItemIndex => currentlyDraggedItemIndex;

    public InventorySlotManager(UIInventoryItem itemPrefab, RectTransform contentPanel, MouseFollower mouseFollower)
    {
        this.itemPrefab = itemPrefab;
        this.contentPanel = contentPanel;
        this.mouseFollower = mouseFollower;
    }

    public void CreateInventorySlots(int count, Action<UIInventoryItem> onItemClicked, Action<UIInventoryItem> onBeginDrag,
        Action<UIInventoryItem> onEndDrag, Action<UIInventoryItem> onDrop, Action<UIInventoryItem> onRightClick)
    {
        for (int i = 0; i < count; i++)
        {
            var item = UnityEngine.Object.Instantiate(itemPrefab, contentPanel);
            listOfUIItem.Add(item);
            item.OnItemClicked += onItemClicked;
            item.OnItemBeginDrag += onBeginDrag;
            item.OnItemEndDrag += onEndDrag;
            item.OnItemDropOn += onDrop;
            item.OnRightMouseBtnClick += onRightClick;
        }
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

    public void ShowItemAction(int index, ItemActionPanel actionPanel)
    {
        if (index >= 0 && index < listOfUIItem.Count)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItem[index].transform.position;
        }
    }

    public void ResetSelection(UIInventoryDescription itemDescription)
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    public void ResetAllItems()
    {
        foreach (var item in listOfUIItem)
        {
            item?.ResetData();
            item?.Deselect();
        }
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    public void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    public void DeselectAllItems()
    {
        foreach (var item in listOfUIItem)
            item.Deselect();
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < listOfUIItem.Count)
            listOfUIItem[index].Select();
    }

    public int GetItemIndex(UIInventoryItem item) => listOfUIItem.IndexOf(item);
}
