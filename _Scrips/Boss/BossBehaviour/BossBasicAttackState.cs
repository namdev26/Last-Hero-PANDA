using UnityEngine;

public class BossBasicAttackState : BossState
{
    BossActionManager actionManager;

    public BossBasicAttackState(BossController boss) : base(boss)
    {
        actionManager = new BossActionManager(boss);
    }

    public override void EnterState()
    {
        boss.animator.Play("BasicAttack");
        boss.isAttacking = false;
        boss.FlipTowardsPlayer();
    }

    public override void UpdateState()
    {

        if (!boss.isAttacking && boss.IsAnimationComplete("BasicAttack"))
        {
            actionManager.ChooseRandomAttack(boss.distanceToPlayer);
        }

        // Kiểm tra điều kiện kết thúc của boss (chết hoặc buff)
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
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }

    //void PerformBasicAttack()
    //{
    //    float distanceToPlayer = Vector2.Distance(boss.transform.position, boss.player.position);
    //    if (distanceToPlayer <= 2f)  // Kiểm tra khoảng cách với player
    //    {
    //        Debug.Log("Boss đang gây sát thương cho Player");
    //        // Gây sát thương thực tế tại đây, có thể thêm logic cho việc giảm HP của player
    //        PlayerHealth player = boss.player.GetComponent<PlayerHealth>();
    //        if (player != null)
    //        {
    //            player.TakeDamage(boss.attackDamage);
    //        }
    //    }
    //}
}
