using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName = "Monster";
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 3f;
    public float attackRange = 1f;
    public float patrolDistance = 15f;
    public float maxIdleTime = 2f; // Th?i gian ??ng yên t?i ?a
    public int maxHealth = 10;
    //public float wakeUpRange = 2f;
    //public bool moveHorizontallyOnly = true; // True: ch? di chuy?n ngang (quái ??t), False: di chuy?n c? X/Y (quái bay)
}