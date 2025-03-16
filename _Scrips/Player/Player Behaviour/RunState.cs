using UnityEngine;
public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.SetFloat("Speed", 1f); // Bật animation chạy
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput == 0 && player.IsGrounded)
        {
            player.ChangeState(new IdleState(player)); // Không có input → quay về Idle
        }
        else
        {
            player.Move(moveInput);
            animator.SetFloat("Speed", Mathf.Abs(moveInput)); // Luôn cập nhật giá trị Speed
        }
    }
    public override void ExitState()
    {
        animator.SetFloat("Speed", 0f);
    }
}
