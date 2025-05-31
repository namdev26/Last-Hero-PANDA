using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Vector2 dashTarget;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private Transform transformBloodEffect;

    [Header("Movement Settings")]
    [SerializeField] private float stopDistance = 2f; // Khoảng cách dừng lại

    private BossActionManager actionManager;
    public float dashSpeed = 10f;
    public int attackDamage = 10;
    public float defense = 5f;
    public float moveSpeed = 5f;

    //bien check boss   
    public bool isAttacking = false;
    public BossState currentState;
    public float distanceToPlayer;
    public bool facingRight = true; // Thêm để track hướng boss đang nhìn

    public void ShowAttackWarning(string attackType)
    {
        // Hiển thị cảnh báo visual/audio ở đây
        Debug.Log($"Boss sẽ thực hiện {attackType}!");
    }

    private void Awake()
    {
        actionManager = new BossActionManager(this);

        // Tự động tìm player với tag "Player"
        FindPlayer();
    }

    private void Start()
    {
        TransitionToState(new BossIdleState(this));
    }

    private void Update()
    {
        // Kiểm tra nếu player bị null thì tìm lại
        if (player == null)
        {
            FindPlayer();
        }

        currentState?.UpdateState();

        // Chỉ tính khoảng cách nếu player không null
        if (player != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.position);
        }

        actionManager.Update(); // Cần thêm dòng này để update phase system
    }

    // Method để tự động tìm player
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Boss đã tìm thấy Player: " + playerObject.name);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameObject nào có tag 'Player'!");
        }
    }

    public void TransitionToState(BossState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void MoveTowardsPlayer()
    {
        // Kiểm tra player có tồn tại không
        if (player == null) return;

        // Kiểm tra khoảng cách trước khi di chuyển
        if (distanceToPlayer <= stopDistance)
        {
            // Dừng lại, chỉ flip về phía player
            rb.velocity = new Vector2(0, rb.velocity.y);
            FlipTowardsPlayer();
            return;
        }

        // Di chuyển bình thường về phía player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        FlipTowardsPlayer();
    }

    public void MoveAwayFromPlayer()
    {
        if (player == null) return;

        // Tính hướng ngược lại với player
        Vector2 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

        // Di chuyển theo hướng ngược lại với tốc độ chậm hơn
        rb.velocity = new Vector2(directionAwayFromPlayer.x * moveSpeed * 0.7f, rb.velocity.y);

        // Flip sprite theo hướng di chuyển
        if (directionAwayFromPlayer.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (directionAwayFromPlayer.x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void FlipTowardsPlayer()
    {
        if (player == null) return;

        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
            (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            this.Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight; // Cập nhật trạng thái hướng
    }

    public bool IsAnimationComplete(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.95f;
    }

    // Thêm method để set khoảng cách dừng từ bên ngoài
    public void SetStopDistance(float distance)
    {
        stopDistance = distance;
    }

    // Method để kiểm tra có đang ở khoảng cách tấn công không
    public bool IsInAttackRange()
    {
        return distanceToPlayer <= stopDistance;
    }

    // Method để force stop movement
    public void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    // Method public để tìm lại player từ bên ngoài nếu cần
    public void RefreshPlayerReference()
    {
        FindPlayer();
    }
}