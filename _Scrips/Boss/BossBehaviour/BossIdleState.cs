using UnityEngine;

public class BossIdleState : BossState
{
    private float idleTime;
    private float timer;
    private BossActionManager actionManager;

    public BossIdleState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
    }

    public override void EnterState()
    {
        boss.animator.Play("Idle");
        idleTime = Random.Range(0.5f, 1f); // Thời gian ngẫu nhiên để boss chuyển trạng thái
        timer = 0f;
        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

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

        if (timer >= idleTime)
        {
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }
    }

    public override void ExitState()
    {
    }
}
