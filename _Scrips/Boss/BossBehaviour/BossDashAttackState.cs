using UnityEngine;

public class BossDashAttackState : BossState
{
    private float dashSpeed = 10f;
    private float dashDuration = 0.9f;
    private float timer = 0f;
    private BossHealth bossHealth;

    public BossDashAttackState(BossController boss) : base(boss)
    {
        bossHealth = boss.GetComponent<BossHealth>();
    }

    public override void EnterState()
    {
        boss.animator.Play("DashAttack");
        boss.isAttacking = false;
        timer = 0f;

        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (!boss.isAttacking)
        {
            Vector2 direction = (boss.player.position - boss.transform.position).normalized;
            direction.y = 0;
            boss.rb.velocity = direction * dashSpeed;
            boss.isAttacking = true;
        }
        if (timer >= dashDuration)
        {
            boss.TransitionToState(new BossBasicAttackState(boss));
        }
    }

    public override void ExitState()
    {
        boss.rb.velocity = Vector2.zero;
        boss.transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, 0);
    }
}
