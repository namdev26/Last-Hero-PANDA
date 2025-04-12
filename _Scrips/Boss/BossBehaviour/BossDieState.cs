using UnityEngine;

public class BossDieState : BossState
{
    private bool hasDied = false;

    public BossDieState(BossController boss) : base(boss)
    {
    }

    public override void EnterState()
    {
        boss.animator.Play("Die");
        hasDied = false;
    }

    public override void UpdateState()
    {
        if (!hasDied && boss.IsAnimationComplete("Die"))
        {
            HandleDeath();
            hasDied = true;
        }
    }

    public override void ExitState()
    {
    }

    void HandleDeath()
    {
        Debug.Log("Boss has died!");
        boss.gameObject.SetActive(false);
    }
}
