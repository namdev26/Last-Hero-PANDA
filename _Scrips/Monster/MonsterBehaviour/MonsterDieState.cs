using UnityEngine;

public class MonsterDieState : MonsterState
{
    private readonly GameObject goldPrefab;
    private readonly float dropRate;
    private bool hasDroppedLoot;

    public MonsterDieState(MonsterController monster, GameObject goldPrefab, float dropRate) : base(monster)
    {
        this.monster = monster;
        this.goldPrefab = goldPrefab;
        this.dropRate = dropRate;
    }

    public override void EnterState()
    {
        Debug.Log("Bắt đầu trạng thái Die");
        animator.Play("Die");
        TryDropLoot();
    }

    public override void UpdateState()
    {
        if (monster.isKnocked) return;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Die") && stateInfo.normalizedTime >= 0.95f)
        {
            monster.DestroyMonster();
        }
    }

    public override void ExitState() { }

    private void TryDropLoot()
    {
        if (hasDroppedLoot || goldPrefab == null) return;

        hasDroppedLoot = true;

        if (Random.value <= dropRate)
        {
            GameObject gold = Object.Instantiate(goldPrefab, monster.transform.position, Quaternion.identity);
        }
    }
}
