using UnityEngine;

public class BossIdleState : BossState
{
    private float idleTime;
    private float timer;
    private BossActionManager actionManager;
    private BossHealth bossHealth;

    public BossIdleState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("Idle");
        idleTime = Random.Range(0.5f, 1f);
        timer = 0f;
        boss.FlipTowardsPlayer();
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

        if (timer >= idleTime)
        {
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }
    }

    public override void ExitState()
    {
    }
}
