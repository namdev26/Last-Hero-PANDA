using System.Collections.Generic;
using UnityEngine;

public class BossActionManager
{
    private BossController boss;

    public BossActionManager(BossController boss)
    {
        this.boss = boss;
    }

    private List<(BossState state, float weight)> GetWeightedAttacks()
    {
        return new List<(BossState state, float weight)>
        {
            (new BossBasicAttackState(boss), 0.5f),
            (new BossChainAttackState(boss), 0.2f),
            (new BossRangeAttackState(boss), 0.3f),
            (new BossDashAttackState(boss), 0.4f),
            (new BossMoveState(boss), 0.1f)
        };
    }

    private List<(BossState state, float weight)> FilterByDistance(float distance, List<(BossState state, float weight)> attacks)
    {
        if (distance <= 3.3f)
            return new List<(BossState state, float weight)> { attacks[0] }; // Basic
        else if (distance <= 5f)
            return new List<(BossState state, float weight)> { attacks[2] }; // Range
        else if (distance <= 8f)
            return new List<(BossState state, float weight)> { attacks[1], attacks[3] }; // Chain, Dash
        else
            return new List<(BossState state, float weight)> { attacks[3], attacks[4] }; // Dash, Move
    }

    public void ChooseRandomAttack(float distanceToPlayer)
    {
        var allAttacks = GetWeightedAttacks();
        var possibleAttacks = FilterByDistance(distanceToPlayer, allAttacks);

        float totalWeight = 0f;
        foreach (var a in possibleAttacks)
            totalWeight += a.weight;

        float rand = Random.value * totalWeight;
        float cumulative = 0f;
        foreach (var a in possibleAttacks)
        {
            cumulative += a.weight;
            if (rand <= cumulative)
            {
                boss.TransitionToState(a.state);
                break;
            }
        }
    }
}
