using UnityEngine;

public class StatItem : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Attack,
        Defence
    }

    public ItemType itemType;
    public int value;

    public void UseItem(PlayerStats player)
    {
        switch (itemType)
        {
            case ItemType.Health:
                player.IncreseMaxHP(value);
                break;
            case ItemType.Attack:
                player.IncreseAttack(value);
                break;
            case ItemType.Defence:
                player.IncreseDefence(value);
                break;
        }

        Destroy(gameObject);
    }
}
