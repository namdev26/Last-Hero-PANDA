using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] public Rigidbody2D _rigidbody;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Transform bloodEffectTranform;

    [SerializeField] public ParticleSystem movementParticle;
    [Range(0, 10)]
    [SerializeField] public float occurAfterVelocity = 1f;
    [Range(0, 0.2f)]
    [SerializeField] public float dustFormationPeriod = 0.1f;

    public bool isInvincible = false;
    public float rollInvincibleTime = 0.4f;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;

    public bool isAttacking = false;
    public bool canDoubleJump { get; private set; }
    public bool IsGrounded;
    public bool InWall { get; private set; }
    public bool IsJumping { get; private set; }
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => animator;
    public bool IsRolling { get; set; }

    public bool CanControl = true;

    public float ComboTimer { get; private set; }
    private readonly float comboResetTime = 1f; // Tăng lên để khớp với animation

    private PlayerState currentState;
    [Header("Roll Settings")]
    [SerializeField] private float rollCooldown = 0.7f; // Tăng thời gian chờ giữa các lần roll
    private float lastRollTime;
    private float lastDownPressTime;
    private bool canSlam;

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

        // Giảm ComboTimer và reset combo
        if (ComboTimer > 0)
        {
            ComboTimer -= Time.deltaTime;
            if (ComboTimer <= 0 && !isAttacking)
            {
                ChangeState(new IdleState(this));
            }
        }

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isAttacking)
            {
                ChangeState(new PowerState(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttacking)
            {
                ChangeState(new JumpSpinAttackState(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isAttacking)
            {
                ChangeState(new AXSkill1(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (IsGrounded)
                ChangeState(new JumpState(this));
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                ChangeState(new JumpState(this));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastRollTime > rollCooldown && !IsRolling)
        {
            if (!isAttacking && IsGrounded) // Chỉ cho phép roll khi đang đứng trên mặt đất
            {
                lastRollTime = Time.time;
                ChangeState(new RollState(this));
            }
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
            _rigidbody.velocity = new Vector2(direction * playerStats.Speed, _rigidbody.velocity.y);
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

    public void Roll()
    {
        float rollSpeed = playerStats.Speed * 5;
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
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, playerStats.JumpForce);
    }

    public void ResetComboTimer()
    {
        ComboTimer = comboResetTime;
    }

    public bool HasPendingAttackInput()
    {
        bool input = false; // Đã xóa pendingAttackInput vì không sử dụng
        return input;
    }

    public void PerformAttack(float damage, float attackRange)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            bool attackFromRight = transform.position.x > enemy.transform.position.x;
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            MonsterHealth monsterHealth = enemy.GetComponent<MonsterHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage + playerStats.Damage, bloodEffectTranform, attackFromRight);
                Debug.Log("Đang đánh boss nè");
            }
            else if (monsterHealth != null)
            {
                monsterHealth.TakeDamage(damage + playerStats.Damage, transform, attackFromRight);
                Debug.Log("Đang đánh quái nè");
            }
        }
    }
}