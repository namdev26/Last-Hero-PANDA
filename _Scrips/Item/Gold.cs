using UnityEngine;

public class Gold : MonoBehaviour
{
    public int amount = 10; // số vàng của item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddGold(amount);
                // Thêm hiệu ứng nhặt vàng sau
                Destroy(gameObject);
            }
        }
    }
}
