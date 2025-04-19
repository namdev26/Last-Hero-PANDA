using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int gold = 0;

    public event Action<int> OnGoldChanged;
    public int Gold => gold;

    void Awake() => ResetStats();

    private void ResetStats()
    {
        gold = 0;
        OnGoldChanged?.Invoke(gold);
    }
    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public bool SpendGold(int cost)
    {
        if (gold < cost) return false;
        gold -= cost;
        OnGoldChanged?.Invoke(gold);
        return true;
    }
}
