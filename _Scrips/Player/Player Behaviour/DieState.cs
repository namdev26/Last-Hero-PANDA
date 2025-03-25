using UnityEngine;

public class DieState : PlayerState
{
    public DieState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.SetBool("IsDie", true);
        player.Rigidbody.velocity = Vector2.zero;
        player.Rigidbody.gravityScale = 0;
        player.enabled = false;
    }
}
