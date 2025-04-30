using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public int baseDamage = 20;
    [SerializeField] public int baseDefence = 20;
    [SerializeField] public int baseSpeed = 6;

    // Lưu trữ bonus từ vật phẩm
    private int bonusDamage = 0;
    private int bonusDefence = 0;
    private int bonusHealth = 0;
    private int bonusSpeed = 0;

    // Sự kiện khi chỉ số thay đổi
    public event Action OnStatsChanged;

    // Chỉ số tổng = cơ bản + bonus
    public int Damage => baseDamage + bonusDamage;
    public int Defence => baseDefence + bonusDefence;
    public int Health => baseMaxHealth + bonusHealth;
    public int Speed => baseSpeed + bonusSpeed;

    // Phương thức để cộng bonus từ vật phẩm
    public void AddStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "SucManh":
                bonusDamage += value;
                Debug.Log($"Damage updated: {Damage}");
                break;
            case "KienCuong":
                bonusDefence += value;
                Debug.Log($"Defence updated: {Defence}");
                break;
            case "BenBi":
                bonusHealth += value;
                Debug.Log($"Health updated: {Health}");
                break;
            case "KheoLeo":
                bonusSpeed += value;
                Debug.Log($"Speed updated: {Speed}");
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
                Debug.Log($"Damage updated: {Damage}");
                break;
            case "KienCuong":
                bonusDefence -= value;
                Debug.Log($"Defence updated: {Defence}");
                break;
            case "BenBi":
                bonusHealth -= value;
                Debug.Log($"Health updated: {Health}");
                break;
            case "KheoLeo":
                bonusSpeed -= value;
                Debug.Log($"Speed updated: {Speed}");
                break;
        }
        OnStatsChanged?.Invoke();
    }

    private void OnEnable()
    {
        OnStatsChanged?.Invoke();
    }
}