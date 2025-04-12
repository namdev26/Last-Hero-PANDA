using UnityEngine;

public class BossChainAttackState : BossState
{
    private float chainRange = 5f;
    private float attackDuration = 0.8f;
    private float timer = 0f;
    private bool hasAttacked = false;

    private BossHealth bossHealth;

    public BossChainAttackState(BossController boss) : base(boss)
    {
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("ChainAttack");
        timer = 0f;
        hasAttacked = false;
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

        timer += Time.deltaTime;

        if (!hasAttacked && timer >= 0.3f && boss.distanceToPlayer <= chainRange)
        {
            PerformChainAttack();
            hasAttacked = true;
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
        Debug.Log("Boss thực hiện Chain Attack!");
        // TODO: Gây damage / VFX / hitbox
    }
}
