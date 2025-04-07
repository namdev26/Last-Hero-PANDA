using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int baseMaxHealth = 100;
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private int baseDefence = 20;
    [SerializeField] private int gold = 0;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Damage { get; private set; }
    public int Defence { get; private set; }
    public int Gold { get { return gold; } } // Getter cho gold

    public event Action<float> OnMaxHealthChanged;
    public event Action<float> OnHealthChanged;
    public event Action<int> OnGoldChanged; // Event cho vàng

    void Awake()
    {
        ResetStats(); // Chuyển từ constructor sang Awake
    }

    private void ResetStats()
    {
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;
        Damage = baseDamage;
        Defence = baseDefence;
        gold = 0; // Reset vàng luôn nếu cần

        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
        OnGoldChanged?.Invoke(gold);
    }

    public void IncreaseMaxHP(int amount)
    {
        MaxHealth += amount;
        CurrentHealth += amount;
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
    }

    public void ReduceHealth(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold); // Thông báo khi vàng thay đổi
    }

    public bool SpendGold(int cost) // Trả về bool để báo thành công/thất bại
    {
        if (gold >= cost)
        {
            gold -= cost;
            OnGoldChanged?.Invoke(gold);
            return true;
        }
        return false;
    }
}