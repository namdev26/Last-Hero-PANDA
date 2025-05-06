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

    // Thêm trường này để lưu trữ giá trị MaxHealth hiện tại
    private float currentMaxHealth;

    // Chúng ta sẽ không gán giá trị cho MaxHealth trực tiếp, mà sử dụng currentMaxHealth
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

    private void HandleHealthChanged(float ratio)
    {
        // Chúng ta không cần gọi sự kiện ở đây, vì nó sẽ được gọi trong các hàm khác
    }

    private void HandleMaxHealthChanged(float max)
    {
        // Không cần gọi lại sự kiện tại đây
    }

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
        // Khởi tạo currentMaxHealth từ playerStats
        currentMaxHealth = playerStats.baseMaxHealth;
        CurrentHealth = currentMaxHealth;
        NotifyHealthChanged();
    }

    public void IncreaseMaxHP(float amount)
    {
        currentMaxHealth += amount; // Tăng max health qua currentMaxHealth
        CurrentHealth += amount;  // Cũng tăng current health tương ứng
        NotifyHealthChanged();
    }

    public void ReduceHealth(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    private void NotifyHealthChanged()
    {
        // Chỉ gọi sự kiện nếu giá trị thay đổi
        OnMaxHealthChanged?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }
}
