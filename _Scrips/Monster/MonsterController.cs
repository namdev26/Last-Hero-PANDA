﻿using System.Collections;
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
    [SerializeField] private PlayerStats playerStats;

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

        if (rb == null) Debug.LogError($"{name}: Rigidbody2D missing!");
        if (animator == null) Debug.LogError($"{name}: Animator missing!");
        if (player == null) Debug.LogWarning($"{name}: Player not found!");

        InitializeStates();
        ChangeState(idleState);
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
        // Xác định hướng dựa trên scale của enemy
        bool attackFromRight = transform.localScale.x < 0;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D col in hitPlayers)
        {
            PlayerHealth target = col.GetComponent<PlayerHealth>();
            float monsterDamage = data.damage * (100 - playerStats.Defence) / 100;
            if (target != null) target.TakeDamage(monsterDamage, attackFromRight);
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
            rb.velocity = Vector2.zero; // Reset trước khi knockback
            rb.AddForce(force, ForceMode2D.Impulse); // Sử dụng impulse để lực mạnh tức thì
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
