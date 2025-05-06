using System;
using System.Collections;
using UnityEngine;

public class MonsterHealth : MonoBehaviour, IHealth
{
    public float maxHP;
    public float currentHP;
    [SerializeField] private MonsterController monster;
    [SerializeField] public PoolObject pool;
    public float knockbackForce = 10f;
    [SerializeField] private Transform transformBloodEffect;

    public bool IsDeath() => currentHP <= 0;
    public float CurrentHealth => currentHP;
    public float MaxHealth => maxHP;

    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;

    void Start()
    {
        maxHP = monster.MonsterData.maxHealth;
        currentHP = maxHP;
        OnMaxHealthChanged?.Invoke(maxHP);
        OnHealthChanged?.Invoke(GetHealthRatio());
    }

    public void TakeDamage(float damage, Transform attackerTransform, bool attackFromRight = false)
    {
        ShowBloodEffect(attackFromRight);

        currentHP = Mathf.Max(currentHP - damage, 0);

        if (currentHP > 0)
        {
            // Nếu còn sống thì knockback + hurt
            Vector2 knockbackDir = (transform.position - attackerTransform.position).normalized;
            monster.ApplyKnockback(knockbackDir * monster.knockbackForce);
            monster.ChangeState(monster.HurtState);
        }
        else
        {
            // Nếu chết thì vào DieState
            monster.ChangeState(monster.DieState);
        }
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

    private float GetHealthRatio()
    {
        return maxHP > 0 ? (float)currentHP / maxHP : 0f;
    }
}
