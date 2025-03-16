using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RollState : PlayerState
{
    private float rollSpeed = 17f;
    private int rollHash = Animator.StringToHash("Roll");
    //private bool wasAirborne;

    public RollState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        player.IsRolling = true;
        animator.SetTrigger("Roll");
        player.Roll(rollSpeed);
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.shortNameHash == rollHash && stateInfo.normalizedTime >= 0.9f)
        {
            if (!player.IsGrounded)
            {
                player.ChangeState(new FallState(player));
            }
            else
            {
                player.ChangeState(Mathf.Abs(player.Rigidbody.velocity.x) > 0.1f ? new RunState(player) : new IdleState(player));
            }
        }
    }

    public override void ExitState()
    {
        player.Rigidbody.gravityScale = 2f; // Khôi phục trọng lực bình thường
        player.IsRolling = false;
    }
}
