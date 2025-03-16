using UnityEngine;

public class HeliSlamState : PlayerState
{
    private float slamSpeed = 25f;
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
            player.Animator.SetTrigger("HeliSlamAttack");
        }

        // Kiểm tra xem Animator đã thực sự chuyển sang HeliSlamAttack chưa
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);
        if (hasExploded && stateInfo.IsName("HeliSlamAttack") && stateInfo.normalizedTime >= 1f)
        {
            player.ChangeState(new IdleState(player));
        }
    }
}
