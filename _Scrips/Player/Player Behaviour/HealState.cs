using UnityEngine;

public class HealState : PlayerState
{
    private float healDuration = 1f;
    private float timer = 0f;
    private PlayerHealth PlayerHealth => player.GetComponent<PlayerHealth>();

    public HealState(PlayerController player) : base(player)
    {
    }

    public override void EnterState()
    {
        animator.Play("Idle"); // Không có anim cũng được
        player._rigidbody.velocity = Vector2.zero;
        PlayerHealth.Heal(50); // Hồi máu
        timer = 0f;

        //player.DisableControl();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= healDuration)
        {
            player.ChangeState(new IdleState(player)); // Trở về trạng thái Idle sau khi hồi máu
        }
    }

    public override void ExitState()
    {
        //player.EnableControl();
        timer = 0f;
    }
}
