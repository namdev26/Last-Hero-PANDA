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
        GameObject.Destroy(boss.gameObject);
    }

    public override void ExitState()
    {
    }

}
