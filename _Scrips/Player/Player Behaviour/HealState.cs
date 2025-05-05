using UnityEngine;

public class HealState : PlayerState
{
    private float healDuration = 2f;
    private float timer = 0f;
    private PlayerHealth PlayerHealth => player.GetComponent<PlayerHealth>();

    public HealState(PlayerController player) : base(player)
    {
    }

    public override void EnterState()
    {
        animator.Play("Heal"); // Phát animation "Heal"
        player._rigidbody.velocity = Vector2.zero; // Đặt vận tốc về 0
        PlayerHealth.Heal(50); // Hồi máu
        timer = 0f;
        player.CanControl = false; // Vô hiệu hóa input di chuyển
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        player._rigidbody.velocity = Vector2.zero;

        if (timer >= healDuration)
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