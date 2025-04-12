using UnityEngine;

public class BossBasicAttackState : BossState
{
    private BossActionManager actionManager;
    private BossHealth bossHealth;

    public BossBasicAttackState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("BasicAttack");
        boss.isAttacking = true;
        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {
        if (bossHealth.IsDeath())
        {
            boss.TransitionToState(new BossDieState(boss));
            return;
        }

        if (bossHealth.CanBuff())
        {
            boss.TransitionToState(new BossBuffState(boss));
            return;
        }

        if (boss.isAttacking && boss.IsAnimationComplete("BasicAttack"))
        {
            boss.isAttacking = false;
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }
}
