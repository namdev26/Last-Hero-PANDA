﻿using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PoolObject pool;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private Transform transformBloodEffect;
    [SerializeField] private PlayerController player;

    public float CurrentHealth => playerStats.CurrentHealth;

    public float MaxHealth => playerStats.MaxHealth;


    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;

    private void Awake()
    {
        playerStats.OnHealthChanged += HandleHealthChanged;
        playerStats.OnMaxHealthChanged += HandleMaxHealthChanged;
    }
    private void HandleHealthChanged(float ratio)
    {
        OnHealthChanged?.Invoke(ratio);
    }
    private void HandleMaxHealthChanged(float max)
    {
        OnMaxHealthChanged?.Invoke(max);
    }

    public void TakeDamage(int damage, bool attackFromRight = false)
    {
        if (player.isInvincible) return;

        ShowBloodEffect(attackFromRight);
        if (playerStats.CurrentHealth > 0)
        {
            playerStats.ReduceHealth(damage);
        }
    }

    public void Heal(int amount)
    {
        playerStats.CurrentHealth = Mathf.Min(playerStats.CurrentHealth + amount, playerStats.MaxHealth);
        OnHealthChanged?.Invoke(playerStats.GetHealthRatio());
    }

    private void ShowBloodEffect(bool attackFromRight)
    {
        GameObject blood = pool.Get(transformBloodEffect.position, Quaternion.identity);
        if (attackFromRight)
        {
            blood.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        StartCoroutine(ReturnToPoolAfterDelay(blood, 1f));
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnToPool(obj);
    }
}
