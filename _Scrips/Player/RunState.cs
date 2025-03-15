using UnityEngine;
public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        Debug.Log("Enter RunState");
        animator.SetFloat("Speed", 1f);
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Debug.Log($"RunState Update - MoveInput: {moveInput}, Velocity X: {player.Rigidbody.velocity.x}");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("RunState: Nhấn Space, chuyển sang RollState");
            player.ChangeState(new RollState(player));
            return;
        }

        if (moveInput == 0)
        {
            Debug.Log("RunState: MoveInput = 0, chuyển sang IdleState");
            player.ChangeState(new IdleState(player));
            return;
        }

        player.Move(moveInput);
    }

    public override void ExitState()
    {
        Debug.Log("Exit RunState");
        animator.SetFloat("Speed", 0f);
    }
}