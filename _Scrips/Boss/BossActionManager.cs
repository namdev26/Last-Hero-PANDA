using System.Collections.Generic;
using UnityEngine;

public class BossActionManager
{
    private BossController boss;

    // Khởi tạo với boss controller
    public BossActionManager(BossController boss)
    {
        this.boss = boss;
    }
    public void ChooseRandomAttack(float distanceToPlayer)
    {
        List<(BossState state, float weight)> weightedAttacks = new List<(BossState, float)>
        {
            // Đòn đánh gần
            (new BossBasicAttackState(boss), 0.5f), // 50% tấn công gần
            (new BossChainAttackState(boss), 0.2f),  // 30% tấn công chuỗi
            // Đòn đánh xa
            (new BossRangeAttackState(boss), 0.3f),  // 20% tấn công xa
            // Đòn đánh theo khoảng cách
            (new BossDashAttackState(boss), 0.4f),  // 40% khi khoảng cách xa
            (new BossMoveState(boss), 0.1f) // 10% khi khoảng cách rất xa
        };

        // Lọc các đòn đánh có thể thực hiện dựa trên khoảng cách
        List<(BossState state, float weight)> possibleAttacks = new List<(BossState, float)>();

        // Thêm các đòn đánh vào danh sách có thể thực hiện theo khoảng cách
        if (distanceToPlayer <= 3.3f)
        {
            possibleAttacks.Add(weightedAttacks[0]); // Basic Attack
        }
        else if (distanceToPlayer <= 5f)
        {
            //possibleAttacks.Add(weightedAttacks[4]);// Move State (di chuyển)
            //possibleAttacks.Add(weightedAttacks[0]);// Basic Attack
            possibleAttacks.Add(weightedAttacks[2]); // Range Attack
        }
        else if (distanceToPlayer <= 8f)
        {
            possibleAttacks.Add(weightedAttacks[3]); // Dash Attack
            //possibleAttacks.Add(weightedAttacks[4]); // Move State (di chuyển)
            possibleAttacks.Add(weightedAttacks[1]); // Chain Attack
        }
        else if (distanceToPlayer > 8f)
        {
            possibleAttacks.Add(weightedAttacks[4]);// Move State (di chuyển)
            possibleAttacks.Add(weightedAttacks[3]); // Dash Attack
        }
        else
        {
            possibleAttacks.Add(weightedAttacks[4]); // Move State (di chuyển)
        }

        // Tính tổng trọng số
        float totalWeight = 0f;
        foreach (var attack in possibleAttacks)
        {
            totalWeight += attack.weight;
        }

        // Sinh số ngẫu nhiên từ 0 đến tổng trọng số
        float randomValue = Random.value * totalWeight;

        float cumulativeWeight = 0f;
        foreach (var attack in possibleAttacks)
        {
            cumulativeWeight += attack.weight;
            if (randomValue <= cumulativeWeight)
            {
                boss.TransitionToState(attack.state);
                break;
            }
        }
    }
}
