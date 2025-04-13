using System.Collections;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    [SerializeField] private MonsterController monster;
    [SerializeField] public PoolObject pool;


    public bool IsDeath() => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform positionEffect, bool attackFromRight = false)
    {
        GameObject blood = pool.Get(positionEffect.position, Quaternion.identity);
        if (attackFromRight)
        {
            blood.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        StartCoroutine(ReturnToPoolAfterDelay(blood, 1f));

        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnToPool(obj);
    }
}
