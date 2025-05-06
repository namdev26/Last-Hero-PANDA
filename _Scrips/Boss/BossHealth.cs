using System;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth
{
    public bool hasBuff = false;

    [SerializeField] public PoolObject pool;
    [SerializeField] private Transform transformBloodEffect;

    // Thông số Boss
    public float maxHP = 1000;
    public float currentHP;

    public float CurrentHealth => currentHP;
    public float MaxHealth => maxHP;

    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;

    private void Start()
    {
        currentHP = maxHP;

        // Gọi sự kiện để UI cập nhật khi bắt đầu
        OnMaxHealthChanged?.Invoke(maxHP);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    public bool IsDeath() => currentHP <= 0;

    public bool CanBuff() => currentHP <= maxHP * 0.3f && !hasBuff;

    public void TakeDamage(float damage, Transform positionEffect, bool attackFromRight = false)
    {
        ShowBloodEffect(attackFromRight);
        if (currentHP > 0)
        {
            currentHP = Mathf.Max(currentHP - damage, 0);
            OnHealthChanged?.Invoke(GetHealthRatio());
        }
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

    private float GetHealthRatio()
    {
        return maxHP > 0 ? (float)currentHP / maxHP : 0f;
    }

    public void Heal(float amount)
    {
        if (currentHP > 0 && currentHP < maxHP)
        {
            currentHP += amount;
        }
    }
}
