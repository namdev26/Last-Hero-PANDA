using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PoolObject pool;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private Transform transformBloodEffect;
    [SerializeField] private PlayerController player;

    public float CurrentHealth;
    private float currentMaxHealth;

    private bool isRegenerating = false;
    private float regenPercentage = 0.02f; // 2% of MaxHealth (theo set ID 3)
    private float regenDuration = 5f; // Hồi mỗi 5 giây (theo set ID 3)
    private bool hasRegenSet = false; // Chỉ true khi set ID 3 đủ 5 món

    // Properties for current health and max health
    float IHealth.CurrentHealth => CurrentHealth;
    public float MaxHealth => currentMaxHealth;

    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;

    private void Awake()
    {
        OnHealthChanged += HandleHealthChanged;
        OnMaxHealthChanged += HandleMaxHealthChanged;
        ResetStats();
    }

    private void Update()
    {
        // Tự động kích hoạt hồi máu nếu có set ID 3 và máu chưa đầy
        if (hasRegenSet && CurrentHealth < MaxHealth && !isRegenerating)
        {
            StartHealthRegeneration();
        }
    }

    private void HandleHealthChanged(float ratio) { }
    private void HandleMaxHealthChanged(float max) { }

    public void TakeDamage(float damage, bool attackFromRight = false)
    {
        if (player.isInvincible) return;

        ShowBloodEffect(attackFromRight);
        if (CurrentHealth > 0)
        {
            ReduceHealth(damage);
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChanged?.Invoke(GetHealthRatio());
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

    public float GetHealthRatio() => MaxHealth > 0 ? (float)CurrentHealth / MaxHealth : 0f;

    private void ResetStats()
    {
        currentMaxHealth = playerStats.baseMaxHealth;
        CurrentHealth = currentMaxHealth;
        NotifyHealthChanged();
    }

    public void IncreaseMaxHP(float amount)
    {
        currentMaxHealth += amount;
        CurrentHealth += amount;
        NotifyHealthChanged();
    }

    public void ReduceHealth(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    private void NotifyHealthChanged()
    {
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    // Bật/tắt set hồi máu (gọi từ UIInventoryPage)
    public void SetRegenSetActive(bool active)
    {
        hasRegenSet = active;
        if (!active && isRegenerating)
        {
            StopCoroutine(HealthRegenerationCoroutine());
            isRegenerating = false;
            Debug.Log("Set hồi máu đã tắt");
        }
        else if (active)
        {
            Debug.Log("Set hồi máu đã kích hoạt");
        }
    }

    // Hồi máu
    public void StartHealthRegeneration()
    {
        if (!isRegenerating && CurrentHealth < MaxHealth && hasRegenSet)
        {
            StartCoroutine(HealthRegenerationCoroutine());
        }
    }

    private IEnumerator HealthRegenerationCoroutine()
    {
        isRegenerating = true;
        float regenAmount = MaxHealth * regenPercentage; // Hồi 2% MaxHealth
        while (CurrentHealth < MaxHealth && hasRegenSet)
        {
            Heal(regenAmount);
            yield return new WaitForSeconds(regenDuration); // Chờ 5 giây
        }
        isRegenerating = false;
    }
}