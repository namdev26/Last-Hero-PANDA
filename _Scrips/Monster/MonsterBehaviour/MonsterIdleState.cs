using UnityEngine;

public class MonsterIdleState : MonsterState
{
    private float idleTime;
    private readonly float maxIdleTime;
    private readonly MonsterHealth monsterHealth;

    public MonsterIdleState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
        maxIdleTime = monster.MonsterData.maxIdleTime;
        monsterHealth = monster.GetComponent<MonsterHealth>();
    }

    public override void EnterState()
    {
        animator.Play("Idle");
        idleTime = 0f;
    }

    public override void UpdateState()
    {
        if (monsterHealth.IsDeath())
        {
            monster.ChangeState(monster.DieState);
            return;
        }

        idleTime += Time.deltaTime;

        if (monster.DistanceToPlayer() < monster.MonsterData.detectionRange)
        {
            monster.ChangeState(monster.ChaseState);
        }
        else if (idleTime >= maxIdleTime)
        {
            monster.ChangeState(monster.PatrolState);
        }
    }

    public override void ExitState() { }
}
