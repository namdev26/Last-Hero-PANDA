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

    public void UseItem(PlayerStats playerStats)
    {
        switch (itemType)
        {
            case ItemType.Health:
                playerStats.IncreaseMaxHP(value);
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