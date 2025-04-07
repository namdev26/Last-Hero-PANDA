using UnityEngine;

public class BossMoveState : BossState
{
    private BossActionManager actionManager;

    public BossMoveState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
    }

    public override void EnterState()
    {
        boss.animator.Play("Run");
    }
    public override void UpdateState()
    {
        if (boss.IsDeath())
        {
            boss.TransitionToState(new BossDieState(boss));
            return;
        }
        if (boss.CanBuff())
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
