using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public int baseDamage = 20;
    [SerializeField] public int baseDefence = 20;
    [SerializeField] public int baseSpeed = 6;
    [SerializeField] private int baseJumpForce = 15;

    // Lưu trữ bonus từ vật phẩm
    private int bonusDamage = 0;
    private int bonusDefence = 0;
    private int bonusHealth = 0;
    private int bonusSpeed = 0;
    private int bonusJumpForce = 0;

    // Sự kiện khi chỉ số thay đổi
    public event Action OnStatsChanged;

    // Chỉ số tổng = cơ bản + bonus
    public int Damage => baseDamage + bonusDamage;
    public int Defence => baseDefence + bonusDefence;
    public int Health => baseMaxHealth + bonusHealth;
    public int Speed => baseSpeed + bonusSpeed;
    public int JumpForce => baseJumpForce + bonusJumpForce;

    // Phương thức để cộng bonus từ vật phẩm
    public void AddStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "SucManh":
                bonusDamage += value;
                Debug.Log($"Damage updated: {Damage} (Bonus: {bonusDamage})");
                break;
            case "KienCuong":
                bonusDefence += value;
                Debug.Log($"Defence updated: {Defence} (Bonus: {bonusDefence})");
                break;
            case "BenBi":
                bonusHealth += value;
                Debug.Log($"Health updated: {Health} (Bonus: {bonusHealth})");
                break;
            case "KheoLeo":
                bonusSpeed += value;
                Debug.Log($"Speed updated: {Speed} (Bonus: {bonusSpeed})");
                break;
            default:
                Debug.LogWarning($"Unknown parameter: {parameterName}");
                break;
        }
        OnStatsChanged?.Invoke();
    }

    // Phương thức để trừ bonus khi tháo vật phẩm
    public void RemoveStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "SucManh":
                bonusDamage -= value;
                Debug.Log($"Damage updated: {Damage} (Bonus: {bonusDamage})");
                break;
            case "KienCuong":
                bonusDefence -= value;
                Debug.Log($"Defence updated: {Defence} (Bonus: {bonusDefence})");
                break;
            case "BenBi":
                bonusHealth -= value;
                Debug.Log($"Health updated: {Health} (Bonus: {bonusHealth})");
                break;
            case "KheoLeo":
                bonusSpeed -= value;
                Debug.Log($"Speed updated: {Speed} (Bonus: {bonusSpeed})");
                break;
            default:
                Debug.LogWarning($"Unknown parameter: {parameterName}");
                break;
        }
        OnStatsChanged?.Invoke();
    }

    // Reset tất cả bonus về 0
    public void ResetAllBonuses()
    {
        bonusDamage = 0;
        bonusDefence = 0;
        bonusHealth = 0;
        bonusSpeed = 0;
        Debug.Log("All bonuses reset to 0");
        OnStatsChanged?.Invoke();
    }

    private void OnEnable()
    {
        OnStatsChanged?.Invoke();
    }
}