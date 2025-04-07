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

}
