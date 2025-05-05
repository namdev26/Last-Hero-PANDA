using Inventory.Model;
using UnityEngine;
using System.Collections.Generic;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    // Danh sách tạm để theo dõi các Item đã xử lý trong frame hiện tại
    private HashSet<Item> processedItems = new HashSet<Item>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && !processedItems.Contains(item))
        {
            //Debug.Log($"Picking up item: {item.name}, Quantity: {item.Quantity}");
            processedItems.Add(item); // Đánh dấu item đã xử lý

            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (remainder == 0)
            {
                item.DestroyItem();
                //Debug.Log($"Item {item.name} destroyed.");
            }
            else
            {
                item.Quantity = remainder;
                //Debug.Log($"Item {item.name} updated quantity to {remainder}.");
            }
        }
    }

    private void LateUpdate()
    {
        // Xóa danh sách processedItems mỗi frame để reset cho frame tiếp theo
        processedItems.Clear();
    }
}