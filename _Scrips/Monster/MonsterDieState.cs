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
        //Debug.Log("B?t ??u tr?ng th�i Die");
        animator.Play("Die"); // Ph�t animation ch?t
    }

    public override void UpdateState()
    {
        // Kh�ng l�m g�, ch? ch? animation k?t th�c
        // Animation Event "DestroyMonster" s? h?y qu�i v?t
    }

    public override void ExitState()
    {
        //Debug.Log("Tho�t tr?ng th�i Die (n?u chuy?n tr?ng th�i kh�c tr??c khi h?y)");
    }
}