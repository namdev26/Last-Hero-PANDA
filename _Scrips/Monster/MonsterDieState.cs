using UnityEngine;

public class MonsterDieState : MonsterState
{
    public MonsterDieState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        animator.Play("Die"); // Phát animation ch?t
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
}