using UnityEngine;
using UnityEngine.Rendering;

public class BossBasicAttackState : BossState
{
    private float attackCooldown = 1.5f;
    private float timer = 0f;
    public BossBasicAttackState(BossController boss) : base(boss)
    {
    }


    public override void EnterState()
    {
        boss.animator.Play("BasicAttack");
        boss.isAttacking = false;
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
        if (!boss.isAttacking && timer >= 0.5f)
        {
            PerformBasicAttack();
            boss.isAttacking = true;
        }
        if (timer >= attackCooldown)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }

    void PerformBasicAttack()
    {
        float distanseToPlayer = Vector2.Distance(boss.transform.position, boss.player.position);
        if (distanseToPlayer <= 2f)
        {
            Debug.Log("Boss đang gây sát thương cho Player");
        }
    }

}
