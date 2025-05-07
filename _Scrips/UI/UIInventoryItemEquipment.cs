using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Inventory.Model;

namespace Inventory.UI
{
    public class UIInventoryItemEquipment : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image itemImage;

        public event Action<EquipmentType> OnRightMouseBtnClick;
        public event Action<UIInventoryItemEquipment> OnItemClicked;

        private ItemSO currentItem;
        private EquipmentType slotType;

        public void Init(EquipmentType type)
        {
            slotType = type;
        }

        public void SetData(Sprite sprite, ItemSO item)
        {
            if (sprite == null || item == null)
            {
                return;
            }

            itemImage.sprite = sprite;
            itemImage.gameObject.SetActive(true);

            currentItem = item;


        }

        public void ResetData()
        {
            if (itemImage != null)
            {
                //itemImage.sprite = null;
                itemImage.gameObject.SetActive(false);
            }

            currentItem = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && currentItem != null)
            {
                OnRightMouseBtnClick?.Invoke(slotType);
            }
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public ItemSO GetItem()
        {
            return currentItem;
        }
    }
}