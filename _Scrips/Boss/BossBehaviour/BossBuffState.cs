using UnityEngine;

public class BossBuffState : BossState
{
    private bool buffApplied;
    private float timer;
    private readonly float buffDuration = 1.3f;
    private readonly float buffApplyTime = 0.5f;

    private BossHealth bossHealth;
    public BossBuffState(BossController boss) : base(boss)
    {
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("Buff");
        buffApplied = false;
        timer = 0f;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (!buffApplied && timer >= buffApplyTime)
        {
            ApplyBuff();
            buffApplied = true;
        }
        if (timer >= buffDuration)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
    }

    private void ApplyBuff()
    {
        boss.attackDamage *= 2;
        boss.defense *= 2f;
        boss.moveSpeed *= 1.5f;
        bossHealth.hasBuff = true;
    }
}
