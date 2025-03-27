using UnityEngine;

public class MonsterPatrolState : MonsterState
{
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 target;
    private float minTimePatrol;
    private float timeSleep;

    public MonsterPatrolState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        Debug.Log("Bắt đầu trạng thái Patrol");
        animator.Play("Patrol");
        pointA = monster.startPos;
        pointB = pointA + Vector2.right * monster.MonsterData.patrolDistance;
        if (target == Vector2.zero) // Chỉ chọn ngẫu nhiên lần đầu
        {
            target = Random.Range(0f, 1f) < 0.5f ? pointA : pointB;
        }
        //Debug.Log($"Patrol từ A: {pointA} đến B: {pointB}, target hiện tại: {target}");
        minTimePatrol = 0f;
        timeSleep = Random.Range(2f, 5f);
        monster.UpdateFacingDirection(target);
    }

    public override void UpdateState()
    {
        if (monster.IsStunned()) // Không làm gì nếu bị stun
        {
            Debug.Log("Quái đang bị stun, không di chuyển");
            return;
        }

        minTimePatrol += Time.deltaTime;
        float speed = monster.MonsterData.patrolSpeed;

        // Phát hiện người chơi thì chuyển sang Chase
        if (monster.player != null && Vector2.Distance(monster.transform.position, monster.player.position) < monster.MonsterData.detectionRange)
        {
            Debug.Log("Phát hiện người chơi, chuyển sang Chase");
            monster.ChangeState(monster.ChaseState);
            return;
        }

        // Hết thời gian tuần tra thì chuyển sang Idle
        if (minTimePatrol >= timeSleep)
        {
            float randomTarget = Random.Range(0f, 1f);
            target = randomTarget < 0.5f ? pointA : pointB;
            //Debug.Log($"Hết thời gian tuần tra, target mới: {target}");
            monster.ChangeState(monster.IdleState);
            return;
        }

        // Tính hướng di chuyển tới target
        Vector2 direction = (target - (Vector2)monster.transform.position).normalized;
        monster.Move(direction, speed); // Dùng Move() thay cho MoveTowards

        // Nếu đến gần target thì chuyển hướng
        if (Vector2.Distance(monster.transform.position, target) < 0.1f)
        {
            //Debug.Log($"Đã đến đích, target mới: {(target == pointA ? pointB : pointA)}");
            target = (target == pointA) ? pointB : pointA;
            monster.UpdateFacingDirection(target);
            monster.ChangeState(monster.IdleState);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Thoát trạng thái Patrol");
    }
}