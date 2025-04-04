using UnityEngine;

public class BossBuffState : BossState
{
    public bool applyBuff;
    public float buffDuration = 2f;
    public float timer = 0f;
    public BossBuffState(BossController boss) : base(boss)
    {
    }

    public override void EnterState()
    {
        boss.animator.Play("Buff");
        applyBuff = false;
        timer = 0f;
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (!applyBuff && timer >= 0.5f)
        {
            this.ApplyBuff();
            applyBuff = true;
        }

        if (timer >= buffDuration)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
    }

    void ApplyBuff()
    {
        boss.attackDamage *= 2;
        boss.defense *= 2f;
        boss.moveSpeed *= 1.5f;
        boss.hasBuff = true;
        Debug.Log("Boss has been buffed!");

    }
}