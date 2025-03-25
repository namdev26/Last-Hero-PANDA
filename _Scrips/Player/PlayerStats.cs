using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private float baseDefence = 20f;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Damage { get; private set; }
    public float Defence { get; private set; }

    public event Action<float> OnMaxHealthChanged;
    public event Action<float> OnHealthChanged;

    public PlayerStats()
    {
        ResetStats();
    }

    private void ResetStats()
    {
        MaxHealth = baseMaxHealth;
        CurrentHealth = MaxHealth;
        Damage = baseDamage;
        Defence = baseDefence;

        // Trigger initial health change
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
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
}