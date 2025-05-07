using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class QuestItemSO : ItemSO, IItemAction
    {
        [field: SerializeField]
        public string ActionName => "Drop";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        [field: SerializeField]
        public string QuestID { get; private set; } // ID của nhiệm vụ liên quan

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            // Hành động Drop: Xóa vật phẩm khỏi inventory
            InventoryController inventory = character.GetComponent<InventoryController>();

            Debug.LogWarning($"InventoryController not found on {character.name}");
            return false;
        }

        public bool PerformUnequipAction(GameObject character)
        {
            return false; // QuestItem không hỗ trợ tháo trang bị
        }
    }
}