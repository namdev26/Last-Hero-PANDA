using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    [SerializeField] protected MonsterData data;
    public MonsterData MonsterData => data;
    protected MonsterState currentState;
    protected Animator animator;
    protected Rigidbody2D rb;
    public Animator Animator => animator;
    public Transform player;
    public Vector2 startPos;
    [SerializeField] protected float health;
    protected bool isStunned;
    protected bool isAttacking;
    public float knockbackForce = 10f;


    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayers;


    // States
    protected MonsterState idleState;
    protected MonsterState patrolState;
    protected MonsterState chaseState;
    protected MonsterState attackState;
    protected MonsterState dieState;
    protected MonsterState hurtState;
    protected MonsterState flySleepState;
    protected MonsterState flyWakeUpState;

    public MonsterState IdleState => idleState;
    public MonsterState PatrolState => patrolState;
    public MonsterState ChaseState => chaseState;
    public MonsterState AttackState => attackState;
    public MonsterState DieState => dieState;
    public MonsterState HurtState => hurtState;
    public MonsterState FlySleepState => flySleepState;
    public MonsterState FlyWakeUpState => flyWakeUpState;

    public void DestroyMonster() => Destroy(gameObject);
    public void StartAttack() => isAttacking = true;
    public void EndAttack() => isAttacking = false;
    public void ResumeMovement() => isStunned = false;
    public bool IsStunned() => isStunned;
    public bool IsAttacking => isAttacking;
    public void StopMovement() => isStunned = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        if (rb == null) Debug.LogError("Rigidbody2D missing!");
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator missing on " + gameObject.name);

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) Debug.LogWarning("Player not found!");

        startPos = transform.position;
        health = data.maxHealth;

        InitializeStates();
        ChangeState(idleState);
    }

    protected virtual void Update()
    {
        if (currentState != null && !isStunned) // Chỉ chạy state logic nếu không bị stun
        {
            currentState.UpdateState();
        }
    }

    protected abstract void InitializeStates();

    public void ChangeState(MonsterState newState)
    {
        if (currentState != null) currentState.ExitState();
        currentState = newState;
        if (currentState != null) currentState.EnterState();
        else Debug.LogError("Null state assigned!");
    }

    public float DistanceToPlayer()
    {
        return player != null ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    }

    public virtual void Move(Vector2 direction, float speed)
    {
        if (isStunned)
        {
            Debug.Log("Stunned, should not move!");
            return;
        }
        //Debug.Log("Moving: " + direction);
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void UpdateFacingDirection(Vector2 targetPosition)
    {
        bool flip = targetPosition.x > transform.position.x;
        transform.rotation = Quaternion.Euler(0, flip ? 0 : 180, 0);
    }

    public virtual void TakeDamage(float damage, Vector2 attackerPos)
    {
        health -= damage;

        Vector2 knockbackDir = (transform.position - (Vector3)attackerPos).normalized;
        rb.velocity = knockbackDir * knockbackForce; // Đẩy lùi
        if (health <= 0)
        {
            Debug.Log("Health <= 0, setting IsDie = true");
            animator.SetBool("IsDie", true); // Set parameter để vào DieState
            ChangeState(dieState);
        }
        else if (!isStunned)
        {
            Debug.Log("Setting Hurt trigger");
            animator.SetTrigger("Hurt"); // Set trigger để vào HurtState
                                         // ChangeState(hurtState);
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.GetComponent<PlayerHealth>() != null)
                player.GetComponent<PlayerHealth>().TakeDamage(10);
            else Debug.Log("Player not found!");
        }
    }
}