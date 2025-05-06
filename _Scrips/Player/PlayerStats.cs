using UnityEngine;
using System;
using Inventory.Model;
using System.Collections.Generic;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [Header("Skill Time")]
    [SerializeField] public float skill1Time = 5f;
    [SerializeField] public float skill2Time = 5f;
    [SerializeField] public float skill3Time = 5f;


    [SerializeField] public float baseMaxHealth = 100;
    [SerializeField] public float baseDamage = 20;
    [SerializeField] public float baseDefence = 20;
    [SerializeField] public float baseSpeed = 6;
    [SerializeField] private float baseJumpForce = 15;
    [SerializeField] private PlayerHealth playerHealth;

    // Lưu trữ bonus từ vật phẩm
    public float bonusDamage = 0;
    public float bonusDefence = 0;
    public float bonusHealth = 0;
    public float bonusSpeed = 0;
    public float bonusJumpForce = 0;



    // Sự kiện khi chỉ số thay đổi
    public event Action OnStatsChanged;

    // Chỉ số tổng = cơ bản + bonus
    public float Damage => baseDamage + bonusDamage;
    public float Defence => baseDefence + bonusDefence;
    public float Health => baseMaxHealth + bonusHealth;
    public float Speed => baseSpeed + bonusSpeed;
    public float JumpForce => baseJumpForce + bonusJumpForce;

    // Phương thức để cộng bonus từ vật phẩm
    public void AddStatBonus(string parameterName, int value)
    {
        switch (parameterName)
        {
            case "SucManh":
                bonusDamage += value;
                //Debug.Log($"Damage updated: {Damage} (Bonus: {bonusDamage})");
                break;
            case "KienCuong":
                bonusDefence += value;
                //Debug.Log($"Defence updated: {Defence} (Bonus: {bonusDefence})");
                break;
            case "BenBi":
                bonusHealth += value;
                //Debug.Log($"Health updated: {Health} (Bonus: {bonusHealth})");
                break;
            case "KheoLeo":
                bonusSpeed += value;
                //Debug.Log($"Speed updated: {Speed} (Bonus: {bonusSpeed})");
                break;
            default:
                //Debug.LogWarning($"Unknown parameter: {parameterName}");
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
                break;
            case "KienCuong":
                bonusDefence -= value;
                break;
            case "BenBi":
                bonusHealth -= value;
                break;
            case "KheoLeo":
                bonusSpeed -= value;
                break;
            default:
                break;
        }
        OnStatsChanged?.Invoke();
    }

    public void ResetAllBonuses()
    {
        bonusDamage = 0;
        bonusDefence = 0;
        bonusHealth = 0;
        bonusSpeed = 0;
        OnStatsChanged?.Invoke();
    }

    // set 15% dame
    public void Set1()
    {
        bonusDamage += baseDamage * 0.15f;
        //Debug.Log($"Damage updated with 15% bonus: {Damage} (Bonus: {bonusDamage})");
        OnStatsChanged?.Invoke();
    }

    //set giảm 30% hồi chiêu và tăng tốc chạy 20% khi Hp dưới 50%
    public void Set2()
    {
        //Set2Effect.SetActive(true);

    }

    public void Set3()
    {
        playerHealth.StartHealthRegeneration();
    }

    //set tăng tất cả chỉ số 10%
    public void Set4()
    {
        bonusDamage += baseDamage * 0.1f;
        bonusDefence += bonusDefence * 0.1f;
        bonusHealth += baseMaxHealth * 0.1f;
        bonusJumpForce += bonusJumpForce * 0.1f;
        bonusSpeed += bonusSpeed * 0.1f;
    }

    public void ApplyBuffShillR()
    {
        baseDamage += 10;
        baseMaxHealth += 100;
        baseDefence += 1;
        baseJumpForce += 1;
        baseSpeed += 1;
    }

    private void OnEnable()
    {
        OnStatsChanged?.Invoke();
    }
}