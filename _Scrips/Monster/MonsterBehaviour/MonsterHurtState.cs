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
    }
}
