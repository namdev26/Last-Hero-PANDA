using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] public Rigidbody2D _rigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject monster;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Transform bloodEffectTranform;

    [SerializeField] private ParticleSystem movementParticle;
    [Range(0, 10)]
    [SerializeField] private float occurAfterVelocity = 1f; // Ngưỡng vận tốc để phát bụi
    [Range(0, 0.2f)]
    [SerializeField] private float dustFormationPeriod = 0.1f; // Khoảng thời gian giữa các lần phát bụi
    private float counter = 0f; // Đếm thời gian để phát bụi định kỳ

    public bool isInvincible = false; // biến bất tử khi roll không nhận damage
    public float rollInvincibleTime = 0.4f; // Thời gian invincible khi roll

    public Transform attackPoint;
    public Transform heliSlamPoint;
    public float attackRange;
    public LayerMask enemyLayers;

    public bool isAttacking = false;
    private float lastDownPressTime;
    private bool canSlam = true;
    public bool canDoubleJump { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool InWall { get; private set; }
    public bool IsJumping { get; private set; }
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => animator;
    public bool IsRolling { get; set; }

    public bool CanControl { get; private set; } = true;

    public void EnableControl() => CanControl = true;
    public void DisableControl() => CanControl = false;

    private PlayerState currentState;
    private float rollCooldown = 0.1f;
    private float lastRollTime;

    private void Start()
    {
        currentState = new IdleState(this);
        currentState.EnterState();


    }
    private void Update()
    {
        if (!CanControl)
        {
            currentState.UpdateState();
            return;
        }
        animator.SetFloat("VelocityY", Rigidbody.velocity.y);
        animator.SetBool("IsGrounded", IsGrounded);
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }
        Move(moveInput);

        if (movementParticle != null && IsGrounded)
        {
            counter += Time.deltaTime;

            if (Mathf.Abs(_rigidbody.velocity.x) > occurAfterVelocity)
            {
                if (counter >= dustFormationPeriod)
                {
                    movementParticle.Emit(5); // phát ra 5 hạt bụi
                    counter = 0;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                ChangeState(new QuickAttackState(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeState(new HealState(this));
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            {
                ChangeState(new JabAttackState(this));
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttacking)
            {
                ChangeState(new JumpSpinAttackState(this));
            }
        }
        // Các hành động khác vẫn có thể ngắt tấn công
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded)
                ChangeState(new JumpState(this));
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                ChangeState(new JumpState(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastRollTime > rollCooldown)
        {
            lastRollTime = Time.time;
            ChangeState(new RollState(this));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (Time.time - lastDownPressTime < 0.3f && canSlam)
            {
                ChangeState(new HeliSlamState(this));
                canSlam = false;
            }
            lastDownPressTime = Time.time;
        }

        if (IsGrounded) canSlam = true;
        currentState.UpdateState();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void Move(float direction)
    {
        if (!IsRolling)
        {
            _rigidbody.velocity = new Vector2(direction * moveSpeed, _rigidbody.velocity.y);
            if (direction != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
                animator.SetFloat("Speed", Mathf.Abs(direction));
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Monster"))
        {
            IsGrounded = true;
            canDoubleJump = true;
            IsJumping = false;
        }
        if (collision.CompareTag("Wall"))
        {
            InWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.CompareTag("Monster"))
        {
            IsGrounded = false;
            IsJumping = true;
        }
        if (collision.CompareTag("Wall"))
        {
            InWall = false;
        }
    }

    public void Roll(float rollSpeed)
    {

        if (IsGrounded)
        {
            _rigidbody.velocity = new Vector2(transform.localScale.x * rollSpeed, _rigidbody.velocity.y);
        }
        else if (IsJumping)
        {
            _rigidbody.velocity = new Vector2(transform.localScale.x * rollSpeed, -0.1f);
        }
        StartCoroutine(EnableInvincibilityForSeconds(rollInvincibleTime));
    }

    private IEnumerator EnableInvincibilityForSeconds(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public void Jump()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpForce);
    }
    public void PerformAttack(int damage, float attackRange)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            bool attackFromRight = transform.position.x > monster.transform.position.x;
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            MonsterHealth monsterHealth = enemy.GetComponent<MonsterHealth>();

            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage + playerStats.Damage, bloodEffectTranform, attackFromRight);
            }
            else if (monsterHealth != null)
            {
                monsterHealth.TakeDamage(damage + playerStats.Damage, transform, attackFromRight);

            }
        }
    }

    public void HeliSlamAttack(int damage, float attackRange)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(heliSlamPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            bool attackFromRight = transform.position.x > monster.transform.position.x;
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            MonsterHealth monsterHealth = enemy.GetComponent<MonsterHealth>();

            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage + playerStats.Damage, bloodEffectTranform, attackFromRight);
            }
            else if (monsterHealth != null)
            {
                monsterHealth.TakeDamage(damage + playerStats.Damage, transform, attackFromRight);

            }
        }
    }

    //public void HealthRecoveryFromItems(Collider2D collider2D)
    //{
    //    if (collider2D.CompareTag("ItemHealth"))
    //    {
    //        playerHealth.Heal(HealthPickup.healAmount);
    //        Debug.Log("Player đã nhặt vật phẩm hồi máu: +20 HP");
    //        Destroy(collider2D.gameObject);
    //    }
    //}

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}