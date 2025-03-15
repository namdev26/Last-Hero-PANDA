using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;
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
        Debug.Log($"Update - Input Space: {Input.GetKeyDown(KeyCode.Space)}, Horizontal: {Input.GetAxisRaw("Horizontal")}, Current State: {currentState?.GetType().Name}");
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastRollTime > rollCooldown)
        {
            lastRollTime = Time.time;
            ChangeState(new RollState(this));
        }
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        Debug.Log($"FixedUpdate - Current State: {currentState?.GetType().Name}");
        currentState.FixedUpdateState();
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
            }
        }
    }
    public void Roll(float rollSpeed)
    {
        _rigidbody.velocity = new Vector2(FacingDirection * rollSpeed, _rigidbody.velocity.y);
    }
}