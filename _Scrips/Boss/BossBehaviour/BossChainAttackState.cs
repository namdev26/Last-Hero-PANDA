using UnityEngine;

public class BossChainAttackState : BossState
{
    private float chainRange = 5f;
    private float attackDuration = 0.8f;
    private float timer = 0f;

    public BossChainAttackState(BossController boss) : base(boss) { }

    public override void EnterState()
    {
        boss.animator.Play("ChainAttack");
        timer = 0f;
        boss.isAttacking = false;
        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer < attackDuration)
        {

            if (!boss.isAttacking)
            {
                boss.isAttacking = true;
            }

            if (boss.distanceToPlayer <= chainRange)
            {
                PerformChainAttack();
            }
        }

        if (timer >= attackDuration)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }

    void PerformChainAttack()
    {

    }
}
