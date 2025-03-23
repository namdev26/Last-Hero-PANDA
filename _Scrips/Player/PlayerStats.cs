using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100; // Máu tối đa
    public float currentHealth; // Máu hiện tại
    public float damage = 20; // Sát thương
    public float defence = 20;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void IncreseMaxHP(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }

    public void IncreseAttack(int amount)
    {
        damage += amount;
    }

    public void IncreseDefence(int amount)
    {
        defence += amount;
    }

}
