using UnityEngine;

public class MonsterDieState : MonsterState
{
    //private MonsterController monster;

    public MonsterDieState(MonsterController monster) : base(monster)
    {
        this.monster = monster;
    }

    public override void EnterState()
    {
        //Debug.Log("B?t ??u tr?ng thái Die");
        animator.Play("Die"); // Phát animation ch?t
    }

    public override void UpdateState()
    {
        // Không làm gì, ch? ch? animation k?t thúc
        // Animation Event "DestroyMonster" s? h?y quái v?t
    }

    public override void ExitState()
    {
        //Debug.Log("Thoát tr?ng thái Die (n?u chuy?n tr?ng thái khác tr??c khi h?y)");
    }
}