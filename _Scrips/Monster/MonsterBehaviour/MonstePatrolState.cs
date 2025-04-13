using UnityEngine;
public class MonsterPatrolState : MonsterState
{
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 target;
    private float patrolTimer;
    private float patrolDuration;

    public MonsterPatrolState(MonsterController monster) : base(monster) { }

    public override void EnterState()
    {
        animator.Play("Patrol");
        pointA = monster.startPos;
        pointB = pointA + Vector2.right * monster.MonsterData.patrolDistance;

        // Nếu cần quay về điểm tuần tra, đặt target là pointA
        if (monster.MustReturnToPatrolPoint)
        {
            target = pointA;
        }
        else if (target == Vector2.zero)
        {
            target = pointB;
        }

        monster.UpdateFacingDirection(target);
        patrolTimer = 0f;
        patrolDuration = Random.Range(2f, 5f);
    }

    public override void UpdateState()
    {
        patrolTimer += Time.deltaTime;
        float speed = monster.MonsterData.patrolSpeed;

        // Nếu đang quay về điểm tuần tra, không phát hiện người chơi
        if (!monster.MustReturnToPatrolPoint && IsPlayerInRange())
        {
            // Kiểm tra khoảng cách trước khi chuyển sang Chase
            float distanceFromPatrolPoint = Vector2.Distance(monster.transform.position, monster.startPos);
            if (distanceFromPatrolPoint <= monster.MonsterData.maxDistanceFromPatrolPoint)
            {
                monster.ChangeState(monster.ChaseState);
                return;
            }
        }

        if (patrolTimer >= patrolDuration && !monster.MustReturnToPatrolPoint)
        {
            target = (Random.Range(0f, 1f) < 0.5f) ? pointA : pointB;
            monster.ChangeState(monster.IdleState);
            return;
        }

        Vector2 dir = (target - (Vector2)monster.transform.position).normalized;
        monster.Move(dir, speed);

        if (Vector2.Distance(monster.transform.position, target) < 0.1f)
        {
            // Nếu đã quay về điểm A, reset trạng thái mustReturnToPatrolPoint
            if (monster.MustReturnToPatrolPoint && target == pointA)
            {
                monster.MustReturnToPatrolPoint = false;
            }

            // Tiếp tục tuần tra bình thường
            target = (target == pointA) ? pointB : pointA;
            monster.UpdateFacingDirection(target);
            monster.ChangeState(monster.IdleState);
        }
    }

    private bool IsPlayerInRange()
    {
        return monster.player != null &&
               Vector2.Distance(monster.transform.position, monster.player.position) < monster.MonsterData.detectionRange;
    }

    public override void ExitState()
    {
        // Không cần làm gì đặc biệt khi thoát
    }
}