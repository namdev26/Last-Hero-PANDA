using UnityEngine;

public class JumpSpinAttackState : PlayerState
{
    private float attackStartTime;
    private float attackDuration = 1f; // Thời gian hồi chiêu
    private int damage = 30; // Dame chiêu
    private float attackRange = 2.2f; // Tầm xa chiêu

    public JumpSpinAttackState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        player.isAttacking = true;
        attackStartTime = Time.time;
        player.Animator.SetTrigger("JumpSpinAttack");
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