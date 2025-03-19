using UnityEngine;

public class JabAttackState : PlayerState
{
    private float attackStartTime;
    private float attackDuration = 0.5f; // Thời gian hồi chiêu
    private int damage = 20; // Dame chiêu
    private float attackRange = 1f; // Tầm xa chiêu

    public JabAttackState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        player.isAttacking = true;
        attackStartTime = Time.time;
        player.Animator.SetTrigger("JabAttack");
        player.PerformAttack(this.damage, this.attackRange);
    }

    public override void UpdateState()
    {
        // Kiểm tra thời gian thay vì dựa vào animation state
        if (Time.time - attackStartTime >= attackDuration)
        {
            player.isAttacking = false;

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
                player.ChangeState(new RunState(player));
            else
                player.ChangeState(new IdleState(player));
        }
    }

    public override void ExitState()
    {
        player.isAttacking = false;
    }
}