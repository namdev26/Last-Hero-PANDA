using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        public List<InventoryItem> initialItems = new List<InventoryItem>();
        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip equipClip;

        private void Start()
        {
            // Tự động tìm inventoryUI nếu chưa được gán


            // Tự động tìm player nếu chưa được gán
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj;
                }
            }

            if (!ValidateReferences()) return;
            PrepareUI();
            PrepareInventoryData();
        }

        private bool ValidateReferences()
        {
            bool isValid = true;

            if (player == null)
            {
                Debug.LogError("Player reference is missing in InventoryController.");
                isValid = false;
            }

            if (inventoryUI == null)
            {
                Debug.LogError("UIInventoryPage reference is missing in InventoryController.");
                isValid = false;
            }

            if (inventoryData == null)
            {
                Debug.LogError("InventorySO reference is missing in InventoryController.");
                isValid = false;
            }

            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource reference is missing in InventoryController. Sound effects will not play.");
                // Không đặt isValid = false vì đây không phải lỗi nghiêm trọng
            }

            return isValid;
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            // Kiểm tra inventoryUI đã được gán chưa, nếu chưa thử tìm lại
            if (inventoryUI == null)
            {
                inventoryUI = FindObjectOfType<UIInventoryPage>();
                if (inventoryUI == null)
                {
                    Debug.LogError("Cannot update inventory UI: UIInventoryPage not found in scene!");
                    return;
                }
                // Nếu tìm thấy, đăng ký lại các sự kiện
                PrepareUI();
            }

            // Kiểm tra inventoryState
            if (inventoryState == null)
            {
                Debug.LogError("Cannot update inventory UI: inventoryState is null");
                return;
            }

            try
            {
                if (inventoryUI != null)
                {
                    inventoryUI.ResetAllItems(false);

                    foreach (var item in inventoryState)
                    {
                        if (item.Value.item != null) // Kiểm tra null cho item
                        {
                            inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                        }
                    }

                    RefreshEquippedItems();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating inventory UI: {e.Message}\n{e.StackTrace}");
            }
        }

        private void RefreshEquippedItems()
        {
            var equippedItems = inventoryUI.GetEquippedItems();
            foreach (var pair in equippedItems)
            {
                if (pair.Value != null)
                {
                    Debug.Log($"Refreshing equipped item: {pair.Value.name} in slot {pair.Key}");
                    inventoryUI.UpdateEquippedItem(pair.Key, pair.Value.ItemImage, pair.Value);
                }
            }
            // Cập nhật chỉ số sau khi làm mới slot trang bị
            PlayerStats stats = player?.GetComponent<PlayerStats>();
            //if (stats != null)
            //{
            //    inventoryUI.Show(player); // Đảm bảo gọi UpdateCharacterStats thông qua Show
            //}
        }

        private void PrepareUI()
        {
            inventoryUI.InitInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformActionoppa(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformActionoppa(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                Debug.LogWarning($"Item at index {itemIndex} is empty.");
                return;
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                bool success = itemAction.PerformAction(player, inventoryItem.itemState);
                if (success)
                {
                    audioSource.PlayOneShot(equipClip);
                    inventoryUI.UseItem(itemIndex, inventoryItem.item);
                }
                else
                {
                    Debug.LogWarning($"PerformAction failed for {inventoryItem.item.name}");
                }
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show(player);
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                    RefreshEquippedItems();
                }
                else
                {
                    inventoryUI.Hide();
                }
            }
        }

        private void OnDestroy()
        {
            inventoryData.OnInventoryUpdated -= UpdateInventoryUI;
            inventoryUI.OnDescriptionRequested -= HandleDescriptionRequest;
            inventoryUI.OnSwapItems -= HandleSwapItems;
            inventoryUI.OnStartDragging -= HandleDragging;
            inventoryUI.OnItemActionRequested -= HandleItemActionRequest;
        }
    }
}