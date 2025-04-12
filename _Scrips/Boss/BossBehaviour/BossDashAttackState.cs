using UnityEngine;

public class BossDashAttackState : BossState
{
    private float dashSpeed = 10f;
    private float dashDuration = 0.6f;
    private float timer = 0f;
    private bool hasDashed = false;

    private Vector2 dashDirection;
    private BossHealth bossHealth;

    public BossDashAttackState(BossController boss) : base(boss)
    {
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("DashAttack");
        timer = 0f;
        hasDashed = false;
        boss.isAttacking = true;

        boss.FlipTowardsPlayer();
        dashDirection = (boss.player.position - boss.transform.position).normalized;
        dashDirection.y = 0;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (!hasDashed)
        {
            boss.rb.velocity = dashDirection * dashSpeed;
            hasDashed = true;
        }

        if (timer >= dashDuration)
        {
            boss.rb.velocity = Vector2.zero;
            boss.isAttacking = false;
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
        boss.rb.velocity = Vector2.zero;
    }
}
