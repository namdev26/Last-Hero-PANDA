using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    public float CurrentHealth => playerStats.CurrentHealth;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;
    public event Action OnPlayerDied;

    private void Awake()
    {
        if (playerStats == null)
        {
            playerStats = new PlayerStats();
        }

        // Đăng ký sự kiện
        playerStats.OnHealthChanged += HandleHealthChanged;
        playerStats.OnMaxHealthChanged += HandleMaxHealthChanged;
    }

    public void TakeDamage(int damage)
    {
        // Áp dụng phòng thủ, đảm bảo ít nhất 1 sát thương
        float finalDamage = Mathf.Max(damage);

        playerStats.ReduceHealth(finalDamage);

        if (playerStats.CurrentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }

    private void HandleHealthChanged(float healthRatio)
    {
        OnHealthChanged?.Invoke(healthRatio);
    }

    private void HandleMaxHealthChanged(float maxHealth)
    {
        OnMaxHealthChanged?.Invoke(maxHealth);
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện
        playerStats.OnHealthChanged -= HandleHealthChanged;
        playerStats.OnMaxHealthChanged -= HandleMaxHealthChanged;
    }
}