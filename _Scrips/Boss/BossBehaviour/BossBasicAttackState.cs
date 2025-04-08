using UnityEngine;

public class BossBasicAttackState : BossState
{
    BossActionManager actionManager;
    private BossHealth bossHealth;
    public BossBasicAttackState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("BasicAttack");
        boss.isAttacking = false;
        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {

        if (!boss.isAttacking && boss.IsAnimationComplete("BasicAttack"))
        {
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }

        // Kiểm tra điều kiện kết thúc của boss (chết hoặc buff)
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
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }
}
