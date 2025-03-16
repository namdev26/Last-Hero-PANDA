using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    public bool canDoubleJump { get; private set; }
    public bool IsGrounded { get; private set; } // Kiểm tra có chạm đất không
    public bool IsJumping { get; private set; } // Kiểm tra có JUMp đất không
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    [SerializeField] private Animator animator;
    private Rigidbody2D _rigidbody;

    public Rigidbody2D Rigidbody => _rigidbody;
    public float FacingDirection => transform.localScale.x;
    public Animator Animator => animator;
    public bool IsRolling { get; set; }

    private PlayerState currentState;
    private float rollCooldown = 0.1f;
    private float lastRollTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentState = new IdleState(this);
        currentState.EnterState();
    }

    private void Update()
    {
        animator.SetFloat("VelocityY", Rigidbody.velocity.y);
        animator.SetBool("IsGrounded", IsGrounded);

        float moveInput = Input.GetAxisRaw("Horizontal");
        Move(moveInput); // Đảm bảo nhân vật có thể di chuyển ngay cả khi ở IdleState

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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastRollTime > rollCooldown)
        {
            lastRollTime = Time.time;
            ChangeState(new RollState(this));
        }

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

            if (direction != 0 && IsGrounded)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
                animator.SetFloat("Speed", Mathf.Abs(direction)); // Cập nhật animation khi di chuyển
            }
            else
            {
                animator.SetFloat("Speed", 0); // Khi dừng, chuyển animation về Idle
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Đang va chạm với đất có thể jump"); // ✅ Debug sẽ xuất hiện
            IsGrounded = true;
            canDoubleJump = true; // Reset double jump khi chạm đất
            IsJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
            IsJumping = true;
        }
    }
    public void Roll(float rollSpeed)
    {
        if (IsGrounded)
        {
            // ✅ Giữ nguyên velocity.y khi trên đất
            _rigidbody.velocity = new Vector2(transform.localScale.x * rollSpeed, _rigidbody.velocity.y);
        }
        else if (IsJumping)
        {
            // ✅ Nếu trên không, roll nhưng không giữ nguyên velocity.y (thêm trọng lực)
            _rigidbody.velocity = new Vector2(transform.localScale.x * rollSpeed, -0.1f);
        }
    }
    public void Jump()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpForce);
    }
}