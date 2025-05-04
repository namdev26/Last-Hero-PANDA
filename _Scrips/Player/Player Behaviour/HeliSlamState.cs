using UnityEngine;

public class HeliSlamState : PlayerState
{
    private float slamSpeed = 45;
    private bool hasExploded = false;


    public HeliSlamState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        player.Rigidbody.velocity = new Vector2(0, -slamSpeed);
        hasExploded = false;
    }

    public override void UpdateState()
    {
        if (player.IsGrounded && !hasExploded)
        {
            hasExploded = true;
        }
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        player.ChangeState(new IdleState(player));

    }
}
