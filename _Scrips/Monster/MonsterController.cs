using System.Collections;
using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    [Header("Data & Components")]
    [SerializeField] protected MonsterData data;
    public MonsterData MonsterData => data;
    protected Animator animator;
    protected Rigidbody2D rb;
    public Animator Animator => animator;
    public Transform player;
    public Vector2 startPos;

    [Header("Combat")]
    public float knockbackForce = 10f;
    public bool isStunned;
    public bool isAttacking;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayers;

    // Không cần SerializeField nữa, sẽ tự động tìm
    private PlayerStats playerStats;

    // Các phần khác giữ nguyên...
    [Header("State Machine")]
    protected MonsterState currentState;
    protected MonsterState idleState;
    protected MonsterState patrolState;
    protected MonsterState chaseState;
    protected MonsterState attackState;
    protected MonsterState dieState;
    protected MonsterState hurtState;
    protected MonsterState flySleepState;
    protected MonsterState flyWakeUpState;

    public bool isKnocked = false;
    public float knockbackDuration = 0.2f;

    public bool mustReturnToPatrolPoint = false;
    public bool MustReturnToPatrolPoint
    {
        get => mustReturnToPatrolPoint;
        set => mustReturnToPatrolPoint = value;
    }

    // Properties
    public bool IsStunned => isStunned;
    public bool IsAttacking => isAttacking;

    // State Accessors
    public MonsterState IdleState => idleState;
    public MonsterState PatrolState => patrolState;
    public MonsterState ChaseState => chaseState;
    public MonsterState AttackState => attackState;
    public MonsterState DieState => dieState;
    public MonsterState HurtState => hurtState;
    public MonsterState FlySleepState => flySleepState;
    public MonsterState FlyWakeUpState => flyWakeUpState;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPos = transform.position;

        // Tự động tìm PlayerStats
        FindPlayerStats();

        if (rb == null) Debug.LogError($"{name}: Rigidbody2D missing!");
        if (animator == null) Debug.LogError($"{name}: Animator missing!");
        if (player == null) Debug.LogWarning($"{name}: Player not found!");

        InitializeStates();
        ChangeState(idleState);
    }

    private void FindPlayerStats()
    {
        // Cách 1: Tìm PlayerStats trên Player GameObject
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        // Cách 2: Nếu không tìm thấy, tìm trong toàn bộ scene
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        // Cách 3: Nếu vẫn không tìm thấy, tìm qua GameManager hoặc singleton
        if (playerStats == null)
        {
            // Giả sử bạn có GameManager chứa PlayerStats
            // GameManager gameManager = GameManager.Instance;
            // if (gameManager != null)
            //     playerStats = gameManager.GetPlayerStats();
        }

        if (playerStats == null)
        {
            Debug.LogError($"{name}: PlayerStats not found! Make sure PlayerStats component exists in the scene.");
        }
        else
        {
            Debug.Log($"{name}: Successfully found PlayerStats on {playerStats.gameObject.name}");
        }
    }

    protected virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    protected abstract void InitializeStates();

    public void ChangeState(MonsterState newState)
    {
        if (currentState == newState) return;

        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }

    public void Move(Vector2 direction, float speed)
    {
        if (isKnocked) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void UpdateFacingDirection(Vector2 targetPosition)
    {
        bool facingRight = targetPosition.x > transform.position.x;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }

    public float DistanceToPlayer()
    {
        return player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    }

    public void Attack()
    {
        // Kiểm tra null trước khi thực hiện attack
        if (attackPoint == null)
        {
            Debug.LogError($"{name}: attackPoint is null! Please assign it in Inspector.");
            return;
        }

        if (data == null)
        {
            Debug.LogError($"{name}: MonsterData is null! Please assign it in Inspector.");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError($"{name}: PlayerStats is null! Unable to calculate damage.");
            return;
        }

        // Xác định hướng dựa trên scale của enemy
        bool attackFromRight = transform.localScale.x < 0;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D col in hitPlayers)
        {
            PlayerHealth target = col.GetComponent<PlayerHealth>();
            if (target != null)
            {
                float monsterDamage = data.damage * (100 - playerStats.Defence) / 100;
                target.TakeDamage(monsterDamage, attackFromRight);
            }
        }
    }

    public void DestroyMonster() => Destroy(gameObject);
    public void StartAttack() => isAttacking = true;
    public void EndAttack() => isAttacking = false;
    public void StopMovement() => isStunned = true;
    public void ResumeMovement() => isStunned = false;

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (!isKnocked)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(KnockbackRoutine());
        }
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
}