using UnityEngine;
public class MonsterAttackState : MonsterState
{
    private float attackCooldown = 1f;
    private float lastAttackTime;
    private MonsterHealth monsterHealth;

    public MonsterAttackState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
        monsterHealth = monster.GetComponent<MonsterHealth>();
    }

    public override void EnterState()
    {
        Debug.Log("Bắt đầu trạng thái Attack");
        animator.Play("Attack");
        lastAttackTime = 0f;
        monster.StartAttack();
    }

    public override void UpdateState()
    {
        // Nếu đã đi quá xa điểm tuần tra, buộc phải quay về
        float distanceFromPatrolPoint = Vector2.Distance(monster.transform.position, monster.startPos);
        if (distanceFromPatrolPoint > monster.MonsterData.maxDistanceFromPatrolPoint)
        {
            monster.MustReturnToPatrolPoint = true;
            monster.ChangeState(monster.PatrolState);
            return;
        }

        if (monsterHealth.IsDeath())
        {
            monster.ChangeState(monster.DieState);
            return;
        }

        float distanceToPlayer = monster.DistanceToPlayer();
        if (distanceToPlayer > monster.MonsterData.detectionRange)
        {
            monster.ChangeState(monster.IdleState);
            return;
        }

        if (distanceToPlayer > monster.MonsterData.attackRange)
        {
            monster.ChangeState(monster.ChaseState);
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    public override void ExitState()
    {
        monster.EndAttack();
    }

    private void Attack()
    {
        monster.UpdateFacingDirection(monster.player.position);
        animator.Play("Attack");
    }
}