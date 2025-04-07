using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    public float CurrentHealth => playerStats.CurrentHealth;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;
    //public event Action OnPlayerDied;
    public GameObject bloodEffect;
    public Transform transformBloodEffect;
    public PlayerController player;

    private void Awake()
    {
        if (playerStats == null)
        {
            playerStats = new PlayerStats();
        }
        playerStats.OnHealthChanged += HandleHealthChanged;
        playerStats.OnMaxHealthChanged += HandleMaxHealthChanged;
    }

    public void TakeDamage(int damage, bool attackFromRight = false)
    {
        if (player.isInvincible) return; // né đòn nếu đang invincible
        GameObject blood = Instantiate(bloodEffect, transformBloodEffect.position, Quaternion.identity);

        if (attackFromRight)
        {
            blood.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (playerStats.CurrentHealth > 0)
        {
            playerStats.ReduceHealth(damage);
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