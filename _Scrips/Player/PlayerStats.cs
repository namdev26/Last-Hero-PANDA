using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public int baseMaxMana = 100;
    [SerializeField] public int baseDamage = 20;
    [SerializeField] public int baseDefence = 20;

    // Lưu trữ bonus từ vật phẩm
    private int bonusDamage = 0;
    private int bonusDefence = 0;

    // Chỉ số tổng = cơ bản + bonus
    public int Damage => baseDamage + bonusDamage;
    public int Defence => baseDefence + bonusDefence;

    // Phương thức để cộng bonus từ vật phẩm
    public void AddStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "Damage":
                bonusDamage += value;
                Debug.Log($"Damage updated: {Damage}");
                break;
            case "Defence":
                bonusDefence += value;
                Debug.Log($"Defence updated: {Defence}");
                break;
                // Thêm các chỉ số khác nếu cần, ví dụ: "MaxHealth", "MaxMana"
        }
    }

    // Phương thức để trừ bonus khi tháo vật phẩm
    public void RemoveStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "Damage":
                bonusDamage -= value;
                Debug.Log($"Damage updated: {Damage}");
                break;
            case "Defence":
                bonusDefence -= value;
                Debug.Log($"Defence updated: {Defence}");
                break;
        }
    }
}