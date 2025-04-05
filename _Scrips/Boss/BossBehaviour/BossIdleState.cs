using UnityEngine;

public class BossIdleState : BossState
{
    private float idleTime;
    private float timer;

    public BossIdleState(BossController boss) : base(boss)
    {
    }

    public override void EnterState()
    {
        boss.animator.Play("Idle");
        idleTime = Random.Range(1f, 2f);
        timer = 0f;
        boss.FlipTowardsPlayer();
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (boss.currentHP <= 0)
        {
            boss.TransitionToState(new BossDieState(boss));
            return;
        }

        if (boss.currentHP <= boss.maxHP * 0.3f && !boss.hasBuff)
        {
            boss.TransitionToState(new BossBuffState(boss));
            return;
        }

        float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.player.position);

        if (timer >= idleTime)
        {
            if (distanceToPlayer > 3.5f && distanceToPlayer <= 4f)
            {
                boss.TransitionToState(new BossRangeAttackState(boss));
            }
            else if (distanceToPlayer <= 4f)
            {
                boss.TransitionToState(new BossBasicAttackState(boss));
            }
            //else if (distanceToPlayer <= 6f)
            //{
            //    boss.TransitionToState(new BossChainAttackState(boss));
            //}
            //else if (distanceToPlayer > 4f && distanceToPlayer <= 7f)
            //{
            //    boss.TransitionToState(new BossDashAttackState(boss));
            //}
            else if (distanceToPlayer > 12f)
            {
                boss.TransitionToState(new BossMoveState(boss));
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }

}
