using UnityEngine;

public class RollState : PlayerState
{
    private float rollSpeed = 20f;
    private int rollHash = Animator.StringToHash("Roll");

    public RollState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        if (player == null || player.Rigidbody == null) return;

        player.IsRolling = true;
        animator.SetTrigger("Roll");

        // Duy trì hướng của nhân vật khi roll
        player.Roll(rollSpeed);
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == rollHash && stateInfo.normalizedTime >= 0.9f)
        {
            player.ChangeState(Mathf.Abs(player.Rigidbody.velocity.x) > 0.1f ? new RunState(player) : new IdleState(player));
        }
    }

    public override void ExitState()
    {
        player.IsRolling = false;
    }
}
