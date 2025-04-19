using UnityEngine;

public class StatItem : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Attack,
        Defence
    }

    [SerializeField] private ItemType itemType;
    [SerializeField] private int value;
    [SerializeField] private PlayerHealth playerHealth;

    public void UseItem(PlayerStats playerStats)
    {
        switch (itemType)
        {
            case ItemType.Health:
                playerHealth.IncreaseMaxHP(value);
                break;
                //case ItemType.Attack:
                //    playerStats.IncreaseDamage(value);
                //    break;
                //case ItemType.Defence:
                //    playerStats.IncreaseDefence(value);
                //    break;
        }

        Destroy(gameObject);
    }
}