using UnityEngine;

public abstract class MonsterState
{
    protected MonsterController monster;
    protected Animator animator;
    public MonsterState(MonsterController monster)
    {
        this.monster = monster;
        this.animator = monster.Animator;
    }
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
}
