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
    [SerializeField] public float health;
    public float knockbackForce = 10f;
    public bool isStunned;
    public bool isAttacking;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayers;

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


    public bool mustReturnToPatrolPoint = false;
    public bool MustReturnToPatrolPoint
    {
        get => mustReturnToPatrolPoint;
        set => mustReturnToPatrolPoint = value;
    }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPos = transform.position;
        health = data.maxHealth;

        if (rb == null) Debug.LogError($"{name}: Rigidbody2D missing!");
        if (animator == null) Debug.LogError($"{name}: Animator missing!");
        if (player == null) Debug.LogWarning($"{name}: Player not found!");

        InitializeStates();
        ChangeState(idleState);
    }

    protected virtual void Update()
    {
        if (currentState != null && !isStunned)
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
        if (isStunned) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void UpdateFacingDirection(Vector2 targetPosition)
    {
        bool facingRight = targetPosition.x > transform.position.x;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    public float DistanceToPlayer()
    {
        return player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    }

    public virtual void TakeDamage(float damage, Vector2 attackerPos)
    {
        health -= damage;

        Vector2 knockbackDir = ((Vector2)transform.position - attackerPos).normalized;
        rb.velocity = knockbackDir * knockbackForce;

        if (health <= 0)
        {
            animator.SetBool("IsDie", true);
            ChangeState(dieState);
        }
        else if (!isStunned)
        {
            animator.SetTrigger("Hurt");
        }
    }

    public void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D col in hitPlayers)
        {
            PlayerHealth target = col.GetComponent<PlayerHealth>();
            if (target != null) target.TakeDamage(10);
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
}
