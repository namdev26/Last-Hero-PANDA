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
}
