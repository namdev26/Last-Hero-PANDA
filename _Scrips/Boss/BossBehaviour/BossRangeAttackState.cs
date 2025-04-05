using UnityEngine;

public class BossRangeAttackState : BossState
{
    private float attackCooldown = 0.9f;
    private float timer = 0f;
    public BossRangeAttackState(BossController boss) : base(boss)
    {
    }

    public override void EnterState()
    {
        boss.animator.Play("RangeAttack");
        boss.rb.velocity = Vector2.zero;
        timer = 0f;
        boss.isAttacking = false;
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (!boss.isAttacking && timer >= 0.5f)
        {
            PerformRangeAttack();
            boss.isAttacking = true;
        }

        if (timer >= attackCooldown)
        {
            boss.TransitionToState(new BossIdleState(boss));
        }
    }

    public override void ExitState()
    {
        boss.isAttacking = false;
    }

    private void PerformRangeAttack()
    {
        // Vị trí và hướng tấn công (trước mặt boss)
        Vector2 attackOrigin = boss.transform.position;
        Vector2 direction = boss.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        float range = 2f; // Tầm tấn công
        float width = 1.5f; // Độ rộng vùng đánh
        Vector2 size = new Vector2(range, width);

        // Dịch vùng đánh ra trước mặt
        Vector2 center = attackOrigin + direction * (range / 2f);

        // Kiểm tra va chạm với player
        Collider2D hit = Physics2D.OverlapBox(center, size, 0f, LayerMask.GetMask("Player"));

        if (hit != null)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(boss.attackDamage);
            }
        }

        // Debug để dễ test vùng đánh
        DebugDrawBox(center, size, Color.red, 0.5f);
    }

    private void DebugDrawBox(Vector2 center, Vector2 size, Color color, float duration)
    {
        Vector2 halfSize = size * 0.5f;
        Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
        Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
}
