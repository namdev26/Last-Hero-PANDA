using UnityEngine;

public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        Debug.Log("Bắt đầu trạng thái Chase");
        animator.Play("Chase");
    }

    public override void UpdateState()
    {
        if (monster.IsStunned()) // Không làm gì nếu bị stun
        {
            Debug.Log("Quái đang bị stun, không đuổi theo");
            return;
        }

        // Đuổi theo người chơi
        monster.UpdateFacingDirection(monster.player.position);
        Vector2 direction = (monster.player.position - monster.transform.position).normalized;
        Vector2 horizontalDirection = new Vector2(direction.x, 0f); // Chỉ di chuyển ngang

        // Dùng Move() thay vì thay đổi transform trực tiếp
        monster.Move(horizontalDirection, monster.MonsterData.chaseSpeed);

        // Chuyển sang Attack nếu đủ gần
        if (monster.DistanceToPlayer() < monster.MonsterData.attackRange)
        {
            Debug.Log("Đủ gần để tấn công, chuyển sang AttackState");
            monster.ChangeState(monster.AttackState);
        }
        // Chuyển sang Idle nếu mất dấu người chơi
        else if (monster.DistanceToPlayer() > monster.MonsterData.detectionRange)
        {
            Debug.Log("Mất dấu người chơi, chuyển sang IdleState");
            monster.ChangeState(monster.IdleState);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Thoát trạng thái Chase");
    }
}