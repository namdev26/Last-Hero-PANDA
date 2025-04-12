using UnityEngine;

public class BossMoveState : BossState
{
    private BossActionManager actionManager;
    private BossHealth bossHealth;
    private float moveTime;
    private float timer;

    public BossMoveState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("Run");
        moveTime = Random.Range(0.8f, 1.5f);
        timer = 0f;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

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

        if (timer >= moveTime)
        {
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }
    }

    public override void ExitState()
    {
    }
}
