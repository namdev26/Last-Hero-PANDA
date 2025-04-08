using System;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth
{

    public bool hasBuff = false;
    [SerializeField] public PoolObject pool;
    public float CurrentHealth => throw new NotImplementedException();

    public float MaxHealth => throw new NotImplementedException();

    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;


    // Thong so Boss
    public int maxHP = 1000;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public bool IsDeath()
    {
        return currentHP <= 0;
    }

    public bool CanBuff()
    {
        return currentHP <= maxHP * 0.3f && !hasBuff;
    }

    public void TakeDamage(int damage, Transform positionEffect, bool attackFromRight = false)
    {
        GameObject blood = pool.Get(positionEffect.position, Quaternion.identity);
        if (attackFromRight)
        {
            blood.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        StartCoroutine(ReturnToPoolAfterDelay(blood, 1f));
        if (currentHP > 0)
        {
            currentHP -= damage;
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnToPool(obj);
    }

}
