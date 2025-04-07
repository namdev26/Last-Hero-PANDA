using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] public Transform player;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Vector2 dashTarget;
    [SerializeField] public GameObject bloodEffect;
    [SerializeField] public Transform transformBloodEffect;
    public float dashSpeed = 10f;


    // Thong so Boss
    public int maxHP = 1000;
    public int currentHP;

    public int attackDamage = 10;
    public float defense = 5f;
    public float moveSpeed = 5f;

    //bien check boss
    public bool hasBuff = false;
    public bool isAttacking = false;

    public BossState currentState;

    public float distanceToPlayer;
    private void Start()
    {
        currentHP = maxHP;
        TransitionToState(new BossIdleState(this));
    }

    private void Update()
    {
        currentState?.UpdateState();
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
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

    public bool IsDeath()
    {
        return currentHP <= 0;
    }

    public bool CanBuff()
    {
        return currentHP <= maxHP * 0.3f && !hasBuff;
    }

    public bool IsAnimationComplete(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }

    public void TakeDamage(int damage, bool attackFromRight = false)
    {
        GameObject blood = Instantiate(bloodEffect, transformBloodEffect.position, Quaternion.identity);

        if (attackFromRight)
        {
            blood.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (currentHP > 0)
        {
            currentHP -= damage;
        }
    }

}
