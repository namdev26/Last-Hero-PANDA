using UnityEngine;

public class MonsterHurtState : MonsterState
{
    public MonsterHurtState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        Debug.Log("Entering HurtState");
        monster.StopMovement(); // Dừng di chuyển
        // Không cần Play("Hurt") vì Animator sẽ tự động vào state Hurt qua trigger
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = monster.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Hurt") && stateInfo.normalizedTime >= 1f)
        {
            Debug.Log("Hurt animation finished");
            if (!monster.Animator.GetBool("IsDie")) // Nếu không chết
            {
                monster.Animator.SetTrigger("ExitHurt"); // Trigger để thoát Hurt
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting HurtState");
        monster.ResumeMovement(); // Tiếp tục di chuyển
    }
}