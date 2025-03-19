using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private MonsterController monster;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
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
