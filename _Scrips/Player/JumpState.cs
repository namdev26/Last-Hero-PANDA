using UnityEngine;

public class JumpState : PlayerState
{
    private bool hasDoubleJumped = false;

    public JumpState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.ResetTrigger("Jump");  // Reset trước để đảm bảo Trigger hoạt động
        animator.SetTrigger("Jump");
        player.Jump();
    }


    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !hasDoubleJumped && !player.IsGrounded)
        {
            hasDoubleJumped = true;
            player.Jump();

            animator.ResetTrigger("Jump");  // Reset trước để đảm bảo Trigger hoạt động
            animator.SetTrigger("Jump");    // Kích hoạt lại animation Jump
        }


        if (player.Rigidbody.velocity.y < 0)  // Khi bắt đầu rơi xuống
            player.ChangeState(new FallState(player));
    }
}
