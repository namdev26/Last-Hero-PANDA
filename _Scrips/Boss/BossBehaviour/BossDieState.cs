using UnityEngine;

public class BossDieState : BossState
{
    public BossDieState(BossController boss) : base(boss)
    {
    }

    public override void EnterState()
    {
        boss.animator.Play("Die");
    }
    public override void UpdateState()
    {
        if (boss.IsAnimationComplete("Die"))
        {
            boss.gameObject.SetActive(false);
        }
    }

    public override void ExitState()
    {
    }

}
