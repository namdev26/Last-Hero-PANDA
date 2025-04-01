using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        //animator.SetBool("IsFalling", true);
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Khi nhấn Roll trên không
        {
            player.ChangeState(new RollState(player));  // Chuyển sang RollState
        }

        if (player.Rigidbody.velocity.y < 0)  // Khi bắt đầu rơi xuống
        {
            player.ChangeState(new FallState(player));
        }


    }
    public override void ExitState()
    {
        //animator.SetBool("IsFalling", false);
    }
}
