using UnityEngine;

public class MonsterPatrolState : MonsterState
{
    //private MonsterController monster;
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
        //Debug.Log("Bắt đầu trạng thái Patrol");
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
        minTimePatrol += Time.deltaTime;
        float speed = monster.MonsterData.patrolSpeed;

        if (monster.player != null && Vector2.Distance(monster.transform.position, monster.player.position) < monster.MonsterData.detectionRange)
        {
            //Debug.Log("Phát hiện người chơi, chuyển sang trạng thái Chase");
            monster.ChangeState(monster.ChaseState);
            return;
        }

        if (minTimePatrol >= timeSleep)
        {
            float randomTarget = Random.Range(0f, 1f);
            target = randomTarget < 0.5f ? pointA : pointB;
            //Debug.Log($"Hết thời gian tuần tra, target mới cho lần sau: {target}");
            monster.ChangeState(monster.IdleState);
            return;
        }

        monster.transform.position = Vector2.MoveTowards(monster.transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(monster.transform.position, target) < 0.1f)
        {
            monster.ChangeState(monster.IdleState);
            if (target == pointA)
            {
                target = pointB;
            }
            else
            {
                target = pointA;
            }
            monster.UpdateFacingDirection(target);
            //Debug.Log($"Đã đến đích, target mới: {target}");
        }

        else
        {
            //Debug.Log("Quái vật bị stun, giữ nguyên vị trí: " + monster.transform.position);
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Thoát trạng thái Patrol");
    }
}