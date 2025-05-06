using UnityEngine;

public class PowerState : PlayerState
{
    private float powerDuration = 1.5f;
    private float timer = 0f;
    private PlayerStats playerStats => player.GetComponent<PlayerStats>();

    public PowerState(PlayerController player) : base(player)
    {
    }

    public override void EnterState()
    {
        animator.Play("Power");
        player._rigidbody.velocity = Vector2.zero; // Đặt vận tốc về 0

        playerStats.ApplyBuffShillR();

        // Áp dụng buff
        //playerStats.bonusDamage += 10;
        //playerStats.bonusHealth += 100;
        //playerStats.bonusDefence += 1;
        //playerStats.bonusJumpForce += 1;
        //playerStats.bonusSpeed += 1;


        timer = 0f;
        player.CanControl = false; // Vô hiệu hóa input di chuyển
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        player._rigidbody.velocity = Vector2.zero;

        if (timer >= powerDuration)
        {
            player.ChangeState(new IdleState(player));
        }
    }

    public override void ExitState()
    {
        player.CanControl = true;
        timer = 0f;
    }
}
