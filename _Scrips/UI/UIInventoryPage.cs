using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIInventoryDescription itemDescription;
    [SerializeField] private MouseFollower mouseFollower;
    List<UIInventoryItem> listOfUIItem = new List<UIInventoryItem>();

    public Sprite image, image2;
    public int quantity;
    public string title, descriptionText;

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

    private void HandleShowItemActions(UIInventoryItem item)
    {

    }

    private void HandleSwap(UIInventoryItem item)
    {
        int index = listOfUIItem.IndexOf(item);
        if (index == -1)
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;
        }
        listOfUIItem[currentlyDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
        listOfUIItem[index].SetData(currentlyDraggedItemIndex == 0 ? image : image2, quantity);
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        int index = listOfUIItem.IndexOf(item);
        if (index == -1) return;
        currentlyDraggedItemIndex = index;

        mouseFollower.Toggle(true);
        mouseFollower.SetData(index == 0 ? image : image2, quantity);
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        itemDescription.SetDescription(image, title, descriptionText);
        listOfUIItem[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        listOfUIItem[0].SetData(image, quantity);
        listOfUIItem[1].SetData(image2, quantity);

    }
    public void Hide()
    {
        gameObject.SetActive(false);

    }
}
