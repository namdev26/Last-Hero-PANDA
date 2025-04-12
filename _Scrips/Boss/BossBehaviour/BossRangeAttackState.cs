using UnityEngine;

public class BossRangeAttackState : BossState
{
    private float attackCooldown = 0.9f;
    private float timer = 0f;
    private bool hasFired = false;

    public BossRangeAttackState(BossController boss) : base(boss) { }

    public override void EnterState()
    {
        boss.animator.Play("RangeAttack");
        boss.rb.velocity = Vector2.zero;
        timer = 0f;
        boss.isAttacking = false;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (!hasFired && timer >= 0.5f)
        {
            hasFired = true;
        }

        if (timer >= attackCooldown)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }


}
