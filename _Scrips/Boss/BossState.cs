using UnityEngine;

public abstract class BossState
{
    protected BossController boss;

    public BossState(BossController boss)
    {
        this.boss = boss;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

}
