using UnityEngine;

public class HurtState : PlayerState
{
    public HurtState(PlayerController player) : base(player) { }

    public override void EnterState()
    {
        animator.SetTrigger("Hurt");
    }


}
