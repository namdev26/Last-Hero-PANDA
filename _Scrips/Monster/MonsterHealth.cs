using System.Collections;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    [SerializeField] private MonsterController monster;
    [SerializeField] public PoolObject pool;
    public float knockbackForce = 10f;
    [SerializeField] private Transform transformBloodEffect;

    public bool IsDeath() => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform attackerTransform, bool attackFromRight = false)
    {
        ShowBloodEffect(attackFromRight);

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (currentHealth > 0)
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
}
