using UnityEngine;

public class QuickAttackState : PlayerState
{
    private float attackStartTime;
    private float attackDuration = 0.33f; // Điều chỉnh theo thời gian animation

    public QuickAttackState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        player.isAttacking = true;
        attackStartTime = Time.time;
        player.Animator.SetTrigger("QuickAttack");
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