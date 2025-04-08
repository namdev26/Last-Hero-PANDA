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
    public int Gold => gold;

    public event Action<float> OnMaxHealthChanged;
    public event Action<float> OnHealthChanged;
    public event Action<int> OnGoldChanged;

    void Awake() => ResetStats();

    public float GetHealthRatio() => MaxHealth > 0 ? (float)CurrentHealth / MaxHealth : 0f;

    private void ResetStats()
    {
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;
        Damage = baseDamage;
        Defence = baseDefence;
        gold = 0;

        NotifyHealthChanged();
        OnGoldChanged?.Invoke(gold);
    }

    public void IncreaseMaxHP(int amount)
    {
        MaxHealth += amount;
        CurrentHealth += amount;
        NotifyHealthChanged();
    }

    public void ReduceHealth(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int cost)
    {
        if (gold < cost) return false;
        gold -= cost;
        OnGoldChanged?.Invoke(gold);
        return true;
    }

    private void NotifyHealthChanged()
    {
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }
}
