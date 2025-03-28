using UnityEngine;

public class MonsterDieState : MonsterState
{
    private readonly GameObject goldPrefab; // biến này gần như const nhưng khá linh hoạt để tránh gây tỉ lệ bị set linh tinh
    private readonly float dropRate; // cũng thế
    private bool hasDroppedLoot = false;

    public MonsterDieState(MonsterController monster, GameObject goldPrefab, float dropRate) : base(monster)
    {
        this.monster = monster;
        this.goldPrefab = goldPrefab;
        this.dropRate = dropRate;
    }

    public override void EnterState()
    {
        animator.Play("Die"); // Phát animation ch?t
        DropLoot();
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Die") && stateInfo.normalizedTime >= 1f)
        {
            monster.DestroyMonster(); // Hủy monster
        }
    }

    public override void ExitState()
    {
    }

    public void DropLoot()
    {
        if (hasDroppedLoot || goldPrefab == null) return;
        float chance = Random.value;
        if (chance <= dropRate)
        {
            GameObject gold = Object.Instantiate(goldPrefab, monster.transform.position, Quaternion.identity);
            gold.GetComponent<Gold>().amount = Random.Range(10, 120);
        }
        hasDroppedLoot = true;
    }
}