using UnityEngine;

public class GroundMonsterController : MonsterController
{
    protected override void InitializeStates()
    {
        idleState = new MonsterIdleState(this);
        patrolState = new MonsterPatrolState(this);
        chaseState = new MonsterChaseState(this);
        attackState = new MonsterAttackState(this);
        dieState = new MonsterDieState(this);
        hurtState = new MonsterHurtState(this);
    }
}