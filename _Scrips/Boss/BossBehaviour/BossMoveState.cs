using UnityEngine;

public class BossMoveState : BossState
{
    private BossActionManager actionManager;
    private BossHealth bossHealth;
    public BossMoveState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("Run");
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

        boss.MoveTowardsPlayer();

        actionManager.ChooseRandomAttack(boss.distanceToPlayer);
    }


    public override void ExitState()
    {
    }
}
