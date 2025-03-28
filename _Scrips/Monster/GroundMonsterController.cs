using UnityEngine;

public class GroundMonsterController : MonsterController
{
    [SerializeField] private GameObject goldPrefab; // Prefab vàng trong Inspector
    [SerializeField] private float goldDropRate = 0.5f; // Tỷ lệ rơi vàng

    protected override void InitializeStates()
    {
        idleState = new MonsterIdleState(this);
        patrolState = new MonsterPatrolState(this);
        chaseState = new MonsterChaseState(this);
        attackState = new MonsterAttackState(this);
        dieState = new MonsterDieState(this, goldPrefab, goldDropRate);
        hurtState = new MonsterHurtState(this);
    }
}