using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    [SerializeField] protected MonsterData data; // D? li?u c?u h�nh
    public MonsterData MonsterData => data;
    protected MonsterState currentState;
    protected Animator animator;
    public Animator Animator => animator;
    public Transform player;
    public Vector2 startPos;
    [SerializeField] protected int health;
    protected bool isStunned;
    protected bool isAttacking;

    //[SerializeField] protected GameObject attackHitbox;
    //[SerializeField] protected MonsterAttackHitbox attackHitboxScript;
    //[SerializeField] protected HealthBar healthBar;

    // C�c tr?ng th�i
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
    //public void ActivateAttackHitbox() => attackHitboxScript?.setAtiveCollider();
    // public void DeactivateAttackHitbox() => attackHitboxScript?.setDeativeCollider();
    public void StartAttack() => isAttacking = true;
    public void EndAttack() => isAttacking = false;
    public bool IsAttacking => isAttacking;
    public void StopMovement() => isStunned = true;
    public void ResumeMovement() => isStunned = false;
    public bool IsStunned() => isStunned;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator missing on " + gameObject.name);

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) Debug.LogWarning("Player not found!");

        startPos = transform.position;
        health = data.maxHealth;

        InitializeStates();
        ChangeState(idleState);
        //healthBar.SetHealth(health, data.maxHealth);
    }

    protected virtual void Update()
    {
        if (currentState != null && !isStunned)
        {
            currentState.UpdateState();
        }
        //healthBar.SetHealth(health, data.maxHealth);
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
        //if (data.moveHorizontallyOnly)
        direction = new Vector2(direction.x, 0f); // Qu�i ??t ch? di chuy?n ngang
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void UpdateFacingDirection(Vector2 targetPosition)
    {
        bool flip = targetPosition.x > transform.position.x;
        transform.rotation = Quaternion.Euler(0, flip ? 0 : 180, 0);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //healthBar.SetHealth(health, data.maxHealth);
        if (health <= 0) ChangeState(dieState);
        else ChangeState(hurtState);
    }
}