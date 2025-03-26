using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected Animator animator;

    public PlayerState(PlayerController player)
    {
        this.player = player;
        this.animator = player.Animator;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
}
