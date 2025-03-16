using UnityEngine;
public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.SetFloat("Speed", 0f);
        player.Move(0); // Reset velocity để tránh giữ trạng thái cũ
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            player.ChangeState(new RunState(player)); // Chuyển sang RunState khi có input
        }
    }
}
