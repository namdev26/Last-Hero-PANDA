using UnityEngine;

public class MonsterAttackState : MonsterState
{
    //private MonsterController monster;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    public MonsterAttackState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        Debug.Log("Bắt đầu trạng thái Attack");
        animator.Play("Attack");
        lastAttackTime = 0f;
        monster.StartAttack(); // Bắt đầu tấn công khi vào trạng thái Attack
        //monster.Attack(10, 0.7f);
    }

    public override void UpdateState()
    {
        // Tấn công nếu hết cooldown
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        // Chuyển sang Chase nếu ra khỏi tầm tấn công
        if (monster.DistanceToPlayer() > monster.MonsterData.attackRange)
        {
            monster.ChangeState(monster.ChaseState);
        }
        // Chuyển sang Idle nếu mất dấu người chơi
        else if (monster.DistanceToPlayer() > monster.MonsterData.detectionRange)
        {
            monster.ChangeState(monster.IdleState);
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Thoát trạng thái Attack");
        monster.EndAttack(); // Kết thúc tấn công khi thoát trạng thái Attack
    }

    private void Attack()
    {
        // Debug.Log("Quái vật tấn công người chơi!");
        animator.Play("Attack");
        monster.UpdateFacingDirection(monster.player.position);
        // Logic gây sát thương đã được xử lý trong hitbox (AttackHitbox)
    }
}