using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private PlayerController player;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            player.ChangeState(new DieState(player));
            return;

        }


        currentHealth -= damage;

        player.ChangeState(new HurtState(player));

        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Player bị tấn công! Máu còn lại: {currentHealth}");
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player hồi máu! Máu hiện tại: {currentHealth}");
    }

    public void IncreaseMaxHealth(int extraHealth)
    {
        maxHealth += extraHealth;
        currentHealth = maxHealth; // Hồi đầy máu khi tăng máu tối đa
        Debug.Log($"Max HP tăng lên {maxHealth}! Máu hiện tại: {currentHealth}");
    }


    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = Mathf.Max(newMaxHealth, 1); // Đảm bảo không xuống dưới 1
        currentHealth = maxHealth; // Hồi đầy máu khi tăng maxHealth
        Debug.Log($"Max HP được đặt lại: {maxHealth}");
    }

}
