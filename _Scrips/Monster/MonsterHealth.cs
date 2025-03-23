using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    [SerializeField] private MonsterController monster;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        monster.Animator.SetTrigger("Hurt");
    }

    void Die()
    {
        monster.Animator.SetBool("IsDie", true);
        GetComponent<Collider2D>().enabled = false;
        this.monster.enabled = false;
        Debug.Log("Monster died!");
    }
}
