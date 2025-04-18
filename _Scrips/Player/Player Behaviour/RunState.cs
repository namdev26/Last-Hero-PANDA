using UnityEngine;

public class RunState : PlayerState
{
    private float counter;

    public RunState(PlayerController player) : base(player)
    {
        counter = 0f;
    }

    public override void EnterState()
    {
        animator.SetFloat("Speed", 1f);
        player.isAttacking = false;
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput == 0 && player.IsGrounded)
        {
            player.ChangeState(new IdleState(player));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            if (player.movementParticle != null && player.IsGrounded)
            {
                counter += Time.deltaTime;

                if (Mathf.Abs(player._rigidbody.velocity.x) > player.occurAfterVelocity)
                {
                    if (counter >= player.dustFormationPeriod)
                    {
                        player.movementParticle.Emit(5); // phát ra 5 hạt bụi
                        counter = 0;
                    }
                }
            }
        }
    }

    public override void ExitState()
    {
        animator.SetFloat("Speed", 0f);
        counter = 0f;
    }
}
