using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] TMP_Text quantityTxt;
        [SerializeField] Image borderImage;  // Sửa tên biến từ boderImage thành borderImage

        public event Action<UIInventoryItem> OnItemClicked, OnItemDropOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool empty = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;  // Sửa false thành true vì item đang trống
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity.ToString();  // Cải thiện cách chuyển đổi số thành chuỗi
            empty = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (empty) return;  // Thêm kiểm tra nếu item trống

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (empty) return;  // Thêm kiểm tra nếu item trống
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDropOn?.Invoke(this);  // Có thể không cần kiểm tra empty ở đây vì có thể drop vào slot trống
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Thêm xử lý kéo nếu cần
            // Ví dụ: cập nhật vị trí của item theo chuột khi kéo
        }
    }
}