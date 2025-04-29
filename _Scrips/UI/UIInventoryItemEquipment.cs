using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Inventory.Model;
using Unity.VisualScripting;

namespace Inventory.UI
{
    public class UIInventoryItemEquipment : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image itemImage;

        public event Action<EquipmentType> OnRightMouseBtnClick;

        private ItemSO currentItem;
        private EquipmentType slotType;

        public void Init(EquipmentType type)
        {
            slotType = type;
        }

        public void SetData(Sprite sprite, ItemSO item)
        {
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true);  // Bật ảnh đúng
                itemImage.sprite = sprite;             // Gán ảnh
            }
            else
            {
                Debug.LogWarning($"ItemImage is not assigned in {gameObject.name}.");
            }

            currentItem = item;
        }


        public void ResetData()
        {
            if (itemImage != null)
            {
                itemImage.sprite = null;
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
        }

        public ItemSO GetItem()
        {
            return currentItem;
        }
    }
}
