using UnityEngine;
public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        animator.Play("Chase");
    }

    public override void UpdateState()
    {
        if (monster.isKnocked) return;
        // Nếu đã đi quá xa điểm tuần tra, buộc phải quay về
        float distanceFromPatrolPoint = Vector2.Distance(monster.transform.position, monster.startPos);
        if (distanceFromPatrolPoint > monster.MonsterData.maxDistanceFromPatrolPoint)
        {
            monster.MustReturnToPatrolPoint = true;
            monster.ChangeState(monster.PatrolState);
            return;
        }

        float distanceToPlayer = monster.DistanceToPlayer();
        if (distanceToPlayer > monster.MonsterData.detectionRange)
        {
            monster.ChangeState(monster.IdleState);
            return;
        }

        if (distanceToPlayer < monster.MonsterData.attackRange)
        {
            monster.ChangeState(monster.AttackState);
            return;
        }

        Vector2 direction = (monster.player.position - monster.transform.position).normalized;
        Vector2 moveDirection = new Vector2(direction.x, 0f);
        monster.UpdateFacingDirection(monster.player.position);
        monster.Move(moveDirection, monster.MonsterData.chaseSpeed);
    }

    public override void ExitState()
    {
        monster.ResumeMovement();
    }
}