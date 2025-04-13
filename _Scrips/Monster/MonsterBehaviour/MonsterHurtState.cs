using UnityEngine;

public class MonsterHurtState : MonsterState
{
    public MonsterHurtState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        monster.StopMovement();
        animator.Play("Hurt");
    }

    public override void UpdateState()
    {
        if (monster.isKnocked) return;
        monster.ResumeMovement();
        if (!monster.isKnocked)
        {
            // Khi knockback kết thúc thì quay lại Idle hoặc Chase
            if (monster.DistanceToPlayer() < monster.MonsterData.detectionRange)
                monster.ChangeState(monster.ChaseState);
            else
                monster.ChangeState(monster.IdleState);
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Hurt") && stateInfo.normalizedTime >= 1f)
        {
            if (!animator.GetBool("IsDie"))
            {
                animator.SetTrigger("ExitHurt");
            }
        }
    }

    public override void ExitState()
    {
        monster.ResumeMovement();
        Debug.Log("Thoát trạng thái Hurt");
    }
}
