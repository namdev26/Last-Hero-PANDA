using UnityEngine;

public class MonsterChaseState : MonsterState
{
    //private MonsterController monster;

    public MonsterChaseState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        //Debug.Log("Bắt đầu trạng thái Chase");
        animator.Play("Chase");
    }

    public override void UpdateState()
    {
        // Đuổi theo người chơi
        // Tính hướng từ quái vật đến người chơi
        monster.UpdateFacingDirection(monster.player.position);
        Vector2 direction = (monster.player.position - monster.transform.position).normalized;
        // Chỉ lấy thành phần X của direction, đặt Y = 0 để không di chuyển theo Y
        Vector2 horizontalDirection = new Vector2(direction.x, 0f);
        // Di chuyển quái vật, giữ Y cố định
        monster.transform.position += (Vector3)horizontalDirection * monster.MonsterData.chaseSpeed * Time.deltaTime;

        // Chuyển sang Attack nếu đủ gần
        if (monster.DistanceToPlayer() < monster.MonsterData.attackRange)
        {
            monster.ChangeState(monster.AttackState);
        }
        // Chuyển sang Idle nếu mất dấu người chơi
        else if (monster.DistanceToPlayer() > monster.MonsterData.detectionRange)
        {
            monster.ChangeState(monster.IdleState);
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Thoát trạng thái Chase");
    }
}