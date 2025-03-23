using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                StatItem item = GetComponent<StatItem>();
                if (item != null)
                {
                    item.UseItem(player);
                }
            }
        }
    }
}
