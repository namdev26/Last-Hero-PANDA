using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] public Transform player;
    [SerializeField] public Rigidbody2D rb;

    // Thong so Boss
    public float maxHP = 1000f;
    public float currentHP;
    public int attackDamage = 10;
    public float defense = 5f;
    public float moveSpeed = 5f;

    //bien check boss
    public bool hasBuff = false;
    public bool isAttacking = false;

    public BossState currentState;

    private void Start()
    {
        currentHP = maxHP;
        TransitionToState(new BossIdleState(this));
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void TransitionToState(BossState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        FlipTowardsPlayer();
    }

    public void FlipTowardsPlayer()
    {
        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
            (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
