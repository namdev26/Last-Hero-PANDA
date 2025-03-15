using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.SetFloat("Speed", 0f); // Animation Idle
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            player.ChangeState(new RunState(player)); // Nếu có input di chuyển, chuyển sang RunState
        }
    }
}
