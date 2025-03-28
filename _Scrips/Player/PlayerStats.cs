using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private float baseDefence = 20f;
    [SerializeField] private int gold = 0;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Damage { get; private set; }
    public float Defence { get; private set; }
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

    public void IncreaseMaxHP(float amount)
    {
        MaxHealth += amount;
        CurrentHealth += amount;
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
    }

    public void ReduceHealth(float damage)
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