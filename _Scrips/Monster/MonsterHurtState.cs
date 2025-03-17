using UnityEngine;

public class MonsterHurtState : MonsterState
{
    //private MonsterController monster;

    public MonsterHurtState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        //Debug.Log("Bắt đầu trạng thái Hurt");
        animator.Play("Hurt"); // Phát animation bị trúng
        //monster.StopMovement(); // Dừng quái vật ngay khi vào trạng thái
    }

    public override void UpdateState()
    {
        // Kiểm tra nếu animation "Hurt" đã kết thúc
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Debug.Log("Animation Hurt kết thúc, quay lại trạng thái trước đó");
            monster.ResumeMovement(); // Tiếp tục di chuyển
            monster.ChangeState(monster.PatrolState); // Quay lại Patrol (hoặc trạng thái trước đó)
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Thoát trạng thái Hurt");
    }
}