using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BossActionManager
{
    private BossController boss;
    private bool canAttack = true;
    private float minCooldown = 2f;     // Tăng thời gian chờ tối thiểu
    private float maxCooldown = 4f;     // Tăng thời gian chờ tối đa
    private float stopMovementDistance = 1.5f;

    // Hệ thống "thở" cho người chơi
    private int consecutiveAttacks = 0;  // Đếm số đợt tấn công liên tiếp
    private int maxConsecutiveAttacks = 2; // Tối đa 2 đợt tấn công liên tiếp
    private float breathingTime = 3f;    // Thời gian "thở" sau chuỗi tấn công
    private bool isInBreathingPhase = false;

    // Hệ thống telegraph/warning
    private float telegraphDuration = 1f; // Thời gian cảnh báo trước khi tấn công
    private bool isTelegraphing = false;

    // Hệ thống escalation
    private float combatTimer = 0f;
    private float phaseTransitionTime = 30f; // Chuyển phase sau 30 giây
    private int currentPhase = 1; // Phase 1: Chậm, Phase 2: Trung bình, Phase 3: Nhanh

    public BossActionManager(BossController boss)
    {
        this.boss = boss;
    }

    public void Update()
    {
        combatTimer += Time.deltaTime;
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        int newPhase = Mathf.Min(3, (int)(combatTimer / phaseTransitionTime) + 1);
        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            AdjustTimingsForPhase();
        }
    }

    private void AdjustTimingsForPhase()
    {
        switch (currentPhase)
        {
            case 1: // Giai đoạn đầu - Chậm và có nhịp
                minCooldown = 2.5f;
                maxCooldown = 4.5f;
                maxConsecutiveAttacks = 1;
                breathingTime = 4f;
                break;
            case 2: // Giai đoạn giữa - Tăng tốc nhẹ
                minCooldown = 2f;
                maxCooldown = 3.5f;
                maxConsecutiveAttacks = 2;
                breathingTime = 3f;
                break;
            case 3: // Giai đoạn cuối - Aggressive hơn nhưng vẫn có nhịp
                minCooldown = 1.5f;
                maxCooldown = 3f;
                maxConsecutiveAttacks = 3;
                breathingTime = 2.5f;
                break;
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        consecutiveAttacks++;

        // Kiểm tra có cần "thở" không
        if (consecutiveAttacks >= maxConsecutiveAttacks)
        {
            yield return boss.StartCoroutine(BreathingPhase());
        }
        else
        {
            // Cooldown bình thường
            float randomCooldown = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(randomCooldown);
        }

        canAttack = true;
    }

    private IEnumerator BreathingPhase()
    {
        isInBreathingPhase = true;
        consecutiveAttacks = 0;

        // Boss có thể chuyển sang trạng thái idle hoặc di chuyển chậm
        boss.TransitionToState(new BossIdleState(boss));

        yield return new WaitForSeconds(breathingTime);

        isInBreathingPhase = false;
    }

    private IEnumerator TelegraphAttack(BossState attackState)
    {
        isTelegraphing = true;

        // Hiển thị cảnh báo (có thể là animation, particle effect, sound)
        boss.ShowAttackWarning(attackState.GetType().Name);

        yield return new WaitForSeconds(telegraphDuration);

        isTelegraphing = false;
        boss.TransitionToState(attackState);
        boss.StartCoroutine(AttackCooldown());
    }

    private List<(BossState state, float weight)> GetWeightedAttacks()
    {
        // Điều chỉnh trọng số theo phase
        float basicWeight = currentPhase == 1 ? 0.6f : 0.4f;
        float chainWeight = currentPhase >= 2 ? 0.3f : 0.1f;
        float dashWeight = currentPhase == 3 ? 0.4f : 0.2f;

        return new List<(BossState state, float weight)>
        {
            (new BossBasicAttackState(boss), basicWeight),
            (new BossChainAttackState(boss), chainWeight),
            (new BossRangeAttackState(boss), 0.3f),
            (new BossDashAttackState(boss), dashWeight),
            (new BossMoveState(boss), 0.15f)
        };
    }


    private List<(BossState state, float weight)> FilterByDistance(float distance, List<(BossState state, float weight)> attacks)
    {
        // Nếu khoảng cách đủ gần, boss sẽ không di chuyển nữa và chỉ tấn công
        if (distance <= stopMovementDistance)
        {
            // Loại bỏ trạng thái di chuyển khi ở gần
            var filteredAttacks = new List<(BossState state, float weight)>();
            foreach (var attack in attacks)
            {
                // Bỏ qua trạng thái di chuyển (BossMoveState)
                if (!(attack.state is BossMoveState))
                {
                    filteredAttacks.Add(attack);
                }
            }

            // Nếu rất gần, ưu tiên basic attack
            if (distance <= 3.3f)
            {
                return new List<(BossState state, float weight)> { attacks[0] }; // Basic
            }
            return filteredAttacks.Count > 0 ? filteredAttacks : new List<(BossState state, float weight)> { attacks[0] };
        }
        // Logic hiện tại cho các khoảng cách khác
        else if (distance <= 3.3f)
        {
            // Gần: Ưu tiên basic nhưng đôi khi di chuyển ra xa
            if (Random.value < 0.2f) // 20% cơ hội di chuyển ra xa
                return new List<(BossState state, float weight)> { attacks[4] }; // Move
            return new List<(BossState state, float weight)> { attacks[0] }; // Basic
        }
        else if (distance <= 5f)
        {
            return new List<(BossState state, float weight)> { attacks[2], attacks[0] }; // Range, Basic
        }
        else if (distance <= 8f)
        {
            return new List<(BossState state, float weight)> { attacks[1], attacks[3], attacks[2] }; // Chain, Dash, Range
        }
        else
        {
            return new List<(BossState state, float weight)> { attacks[3], attacks[4] }; // Dash, Move
        }
    }

    public void ChooseRandomAttack(float distanceToPlayer)
    {
        if (!canAttack || isTelegraphing || isInBreathingPhase) return;

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
                // Sử dụng telegraph system cho một số tấn công mạnh
                if (ShouldTelegraph(a.state))
                {
                    boss.StartCoroutine(TelegraphAttack(a.state));
                }
                else
                {
                    boss.TransitionToState(a.state);
                    boss.StartCoroutine(AttackCooldown());
                }
                break;
            }
        }
    }

    private bool ShouldTelegraph(BossState state)
    {
        // Telegraph cho các tấn công mạnh như Dash và Chain
        return state is BossDashAttackState || state is BossChainAttackState;
    }

    // Method để kiểm tra trạng thái hiện tại
    public bool IsInBreathingPhase() => isInBreathingPhase;
    public int GetCurrentPhase() => currentPhase;
    public int GetConsecutiveAttacks() => consecutiveAttacks;
}